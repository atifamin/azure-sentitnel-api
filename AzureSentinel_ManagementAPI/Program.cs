using System;
using System.Threading.Tasks;
using AzureSentinel_ManagementAPI.Actions;
using AzureSentinel_ManagementAPI.AlertRules;
using AzureSentinel_ManagementAPI.AlertRuleTemplates;
using AzureSentinel_ManagementAPI.Bookmarks;
using AzureSentinel_ManagementAPI.DataConnectors;
using AzureSentinel_ManagementAPI.Incidents;
using AzureSentinel_ManagementAPI.Incidents.Models;
using AzureSentinel_ManagementAPI.Infrastructure.Authentication;
using AzureSentinel_ManagementAPI.Infrastructure.Configuration;
using AzureSentinel_ManagementAPI.Infrastructure.SharedModels.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureSentinel_ManagementAPI
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var rawConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var configuration = rawConfig.GetSection("AzureSentinelAPI").Get<AzureSentinelApiConfiguration>();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<AppHost>()
                .AddSingleton<AzureSentinelApiConfiguration>(configuration)
                .AddTransient<AlertRulesController>()
                .AddTransient<AuthenticationService>()
                .AddTransient<AlertRuleTemplatesController>()
                .AddTransient<IncidentsController>()
                .AddTransient<ActionsController>()
                .AddTransient<BookmarksController>()
                .AddTransient<DataConnectorsController>()
                .BuildServiceProvider();

            await serviceProvider.GetService<AppHost>().Run(args);
        }
    }

    class AppHost
    {
        private readonly AuthenticationService _authenticationService;
        private readonly AlertRulesController _alertRulesController;
        private readonly AlertRuleTemplatesController _alertRuleTemplatesController;
        private readonly IncidentsController _incidentsController;
        private readonly ActionsController _actionsController;
        private readonly BookmarksController _bookmarksController;
        private readonly DataConnectorsController _dataConnectorsController;

        // List<Task<string>> tasks = new List<Task<string>>();

        public AppHost(
            AlertRulesController alertRulesController,
            AuthenticationService authenticationService,
            AlertRuleTemplatesController alertRuleTemplatesController,
            IncidentsController incidentsController,
            ActionsController actionsController,
            BookmarksController bookmarksController, DataConnectorsController dataConnectorsController)
        {
            _alertRulesController = alertRulesController;
            _authenticationService = authenticationService;
            _alertRuleTemplatesController = alertRuleTemplatesController;
            _incidentsController = incidentsController;
            _actionsController = actionsController;
            _bookmarksController = bookmarksController;
            _dataConnectorsController = dataConnectorsController;
        }

        public async Task Run(string[] args)
        {
            while (true)
            {
                PrintMenu();

                Console.Write("Option: ");
                var option = Console.ReadLine();

                var valid = int.TryParse(option, out var index);
                valid = valid && index > 0 && index < 29;
                if (!valid)
                {
                    Console.WriteLine("Invalid option... please, press enter to continue...");
                    Console.ReadLine();
                    continue;
                }

                var response = "";
                var input = "";
                try
                {
                    switch (index)
                    {
                        case 1:
                            {
                                Console.WriteLine("Enter Alert rule name");
                                input = Console.ReadLine();
                                response = await _actionsController.CreateAction(input);
                                break;
                            }
                        case 2:
                            {
                                Console.WriteLine("Enter Alert rule name");
                                input = Console.ReadLine();
                                response = await _actionsController.DeleteAction(input);
                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine("Enter Alert rule name");
                                input = Console.ReadLine();
                                response = await _actionsController.GetActionById(input);
                                break;
                            }
                        case 4:
                            {
                                Console.WriteLine("Enter Alert rule name");
                                input = Console.ReadLine();
                                response = await _actionsController.GetActionsByRule(input);
                                break;
                            }

                        case 5:
                            {
                                Console.WriteLine("Enter template name");
                                input = Console.ReadLine();
                                response = await _alertRuleTemplatesController.GetAlertRuleTemplateById(input);
                                break;
                            }
                        case 6:
                            {
                                response = await _alertRuleTemplatesController.GetAlertRuleTemplates();
                                break;
                            }

                        case 7:
                            {
                                Console.WriteLine("Enter alert rule template name");
                                input = Console.ReadLine();
                                response = await _alertRulesController.CreateFusionAlertRule(input);
                                break;
                            }
                        case 8:
                            {
                                Console.WriteLine("Enter rule name");
                                input = Console.ReadLine();
                                response = await _alertRulesController.CreateMicrosoftSecurityIncidentCreationAlertRule(input);
                                break;
                            }
                        case 9:
                            {
                                response = await _alertRulesController.CreateScheduledAlertRule();
                                break;
                            }
                        case 10:
                            {
                                response = await _alertRulesController.DeleteAlertRule();
                                break;
                            }
                        case 11:
                            {
                                response = await _alertRulesController.GetAlertRules();
                                break;
                            }
                        case 12:
                            {
                                response = await _alertRulesController.GetFusionAlertRule();
                                break;
                            }
                        case 13:
                            {
                                response = await _alertRulesController.GetMicrosoftSecurityIdentityCreationAlertRule();
                                break;
                            }
                        case 14:
                            {
                                response = await _alertRulesController.GetScheduledAlertRule();
                                break;
                            }

                        case 15:
                            {
                                response = await _bookmarksController.CreateBookmark();
                                break;
                            }
                        case 16:
                            {
                                response = await _bookmarksController.DeleteBookmark();
                                break;
                            }
                        case 17:
                            {
                                response = await _bookmarksController.GetBookmarkById();
                                break;
                            }
                        case 18:
                            {
                                response = await _bookmarksController.GetBookmarks();
                                break;
                            }

                        case 19:
                            {
                                response = await _dataConnectorsController.GetDataConnectors();
                                break;
                            }
                        case 20:
                            {
                                response = await _dataConnectorsController.DeleteDataConnector();
                                break;
                            }
                        case 21:
                            {
                                response = await _dataConnectorsController.CreateDataConnector();
                                break;
                            }

                        case 22:
                            {
                                Console.WriteLine("Enter Severity option \n 0 High \n 1 Medium \n 2 Low \n 3 Informational");
                                input = Console.ReadLine();
                                Severity severity = (Severity)Convert.ToInt32(input);

                                Console.WriteLine("Enter Incident status \n 0 New \n 1 Active \n 2 Close");
                                input = Console.ReadLine();
                                IncidentStatus status = (IncidentStatus)Convert.ToInt32(input);

                                Console.WriteLine("Enter Incident name");
                                var title = Console.ReadLine();

                                var payload = new IncidentPayload
                                {
                                    PropertiesPayload = new IncidentPropertiesPayload
                                    {
                                        Severity = severity,
                                        Status = status,
                                        Title = title
                                    }
                                };

                                response = await _incidentsController.CreateIncident(payload, title);
                                break;
                            }
                        case 23:
                            {
                                response = await _incidentsController.DeleteIncident();
                                break;
                            }
                        case 24:
                            {
                                response = await _incidentsController.GetIncidentById();
                                break;
                            }
                        case 25:
                            {
                                response = await _incidentsController.GetIncidents();
                                break;
                            }

                        case 26:
                            {
                                response = await _incidentsController.CreateIncidentComment();
                                break;
                            }
                        case 27:
                            {
                                response = await _incidentsController.GetAllIncidentComments();
                                break;
                            }
                        case 28:
                            {
                                response = await _incidentsController.GetIncidentCommentById();
                                break;
                            }
                    }

                    Console.WriteLine(JToken.Parse(response).ToString(Formatting.Indented));
                }
                catch (JsonReaderException exception)
                {
                    if (string.IsNullOrEmpty(response))
                    {
                        Console.WriteLine("Deleted");
                        Console.WriteLine("Enter to continue");
                        Console.ReadLine();
                        continue;
                    }

                    Console.WriteLine(response);
                }

                catch (Exception exception)
                {
                    var currentColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    await Console.Error.WriteLineAsync(exception.Message);
                    Console.ForegroundColor = currentColor;
                }

                Console.WriteLine();
                Console.WriteLine("Enter to continue");
                Console.ReadLine();
            }
        }

        public void PrintMenu()
        {
            Console.WriteLine("Actions:");
            Console.WriteLine("1.- CreateActionOfAlertRule");
            Console.WriteLine("2.- DeleteActionOfAlertRule");
            Console.WriteLine("3.- GetActionOfAlertRuleById");
            Console.WriteLine("4.- GetAllActionsByAlertRule");
            Console.WriteLine();

            Console.WriteLine("Alert rule templates:");
            Console.WriteLine("5.- GetAlertRuleTemplateById");
            Console.WriteLine("6.- GetAlertRuleTemplates");
            Console.WriteLine();

            Console.WriteLine("Alert rules:");
            Console.WriteLine("7.- CreateFusionAlertRule");
            Console.WriteLine("8.- CreateMicrosoftSecurityIncidentCreationAlertRule");
            Console.WriteLine("9.- CreateScheduledAlertRule");
            Console.WriteLine("10.- DeleteAlertRule");
            Console.WriteLine("11.- GetAllAlertRules");
            Console.WriteLine("12.- GetFusionAlertRule");
            Console.WriteLine("13.- GetMicrosoftSecurityIncidentCreationAlertRule");
            Console.WriteLine("14.- GetScheduledAlertRule");
            Console.WriteLine();

            Console.WriteLine("Bookmarks:");
            Console.WriteLine("15.- CreateBookmark");
            Console.WriteLine("16.- DeleteBookmark");
            Console.WriteLine("17.- GetBookmarkById");
            Console.WriteLine("18.- GetBookmarks");
            Console.WriteLine();

            Console.WriteLine("DataConnectors:");
            Console.WriteLine("19.- GetDataConnectors");
            Console.WriteLine("20.- DeleteDataConnector");
            Console.WriteLine("21.- CreateDataConnector");
            Console.WriteLine();

            Console.WriteLine("Incidents:");
            Console.WriteLine("22.- CreateIncident");
            Console.WriteLine("23.- DeleteIncident");
            Console.WriteLine("24.- GetIncidentById");
            Console.WriteLine("25.- GetIncidents");
            Console.WriteLine();

            Console.WriteLine("Incident comments:");
            Console.WriteLine("26.- CreateIncidentComment");
            Console.WriteLine("27.- GetAllIncidentComments");
            Console.WriteLine("28.- GetIncidentCommentById");
            Console.WriteLine();

            Console.WriteLine("Ctrl + C to exit");
        }
    }
}