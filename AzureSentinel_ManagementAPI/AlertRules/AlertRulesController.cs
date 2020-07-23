using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using AzureSentinel_ManagementAPI.AlertRules.Models;
using AzureSentinel_ManagementAPI.Infrastructure.Authentication;
using AzureSentinel_ManagementAPI.Infrastructure.Configuration;
using AzureSentinel_ManagementAPI.Infrastructure.SharedModels.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSentinel_ManagementAPI.AlertRules
{
    public class AlertRulesController
    {
        private const string MICROSOFT_SECURITY_INCIDENT_CREATION_RULE_NAME = "MSICR-1";
        private const string SCHEDULED_ALERT_RULE_NAME = "schueduled-alert-rule-1";

        private readonly AzureSentinelApiConfiguration _azureConfig;
        private readonly AuthenticationService _authenticationService;

        public AlertRulesController(AzureSentinelApiConfiguration azureConfig,
            AuthenticationService authenticationService)
        {
            _azureConfig = azureConfig;
            _authenticationService = authenticationService;
        }

        public async Task<string> CreateFusionAlertRule(string templateId)
        {
            try
            {
                var payload = new FusionAlertRulePayload
                {
                    PropertiesPayload = new FusionAlertRulePropertiesPayload
                    {
                        AlertRuleTemplateName = templateId,
                        Enabled = true,
                    }
                };

                _azureConfig.LastCreatedAction = Guid.NewGuid().ToString();
                var url =
                    $"{_azureConfig.BaseUrl}/alertRules/{_azureConfig.LastCreatedAction}?api-version={_azureConfig.ApiVersion}";

                var serialized = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                });

                var request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = new StringContent(serialized, Encoding.UTF8, "application/json")
                };
                await _authenticationService.AuthenticateRequest(request);

                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

                var error = await response.Content.ReadAsStringAsync();
                var formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error calling the API: \n" +
                                       JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: \n" + ex.Message);
            }
        }

        public async Task<string> CreateMicrosoftSecurityIncidentCreationAlertRule(string ruleId)
        {
            try
            {
                var payload = new SecurityIncidentCreationAlertRulePayload
                {
                    PropertiesPayload = new SecurityIncidentCreationAlertRulePropertiesPayload
                    {
                        ProductFilter = ProductFilter.AzureSecurityCenter,
                        DisplayName = MICROSOFT_SECURITY_INCIDENT_CREATION_RULE_NAME,
                        Enabled = true
                    }
                };

                var url =
                    $"{_azureConfig.BaseUrl}/alertRules/{MICROSOFT_SECURITY_INCIDENT_CREATION_RULE_NAME}?api-version={_azureConfig.ApiVersion}";

                var serialized = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                });

                var request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = new StringContent(serialized, Encoding.UTF8, "application/json")
                };
                await _authenticationService.AuthenticateRequest(request);

                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

                var error = await response.Content.ReadAsStringAsync();
                var formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error calling the API: \n" +
                                       JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: \n" + ex.Message);
            }
        }

        public async Task<string> CreateScheduledAlertRule()
        {
            try
            {
                var payload = new ScheduledAlertRulePayload
                {
                    PropertiesPayload = new ScheduledAlertRulePropertiesPayload
                    {
                        Query = "Syslog",
                        QueryFrequency = "PT5M",
                        QueryPeriod = "PT5M",
                        Severity = Severity.Low,
                        TriggerOperator = TriggerOperator.Equal,
                        TriggerThreshold = 1,
                        SuppressionDuration = "PT5M",
                        SuppressionEnabled = true,
                        DisplayName = SCHEDULED_ALERT_RULE_NAME,
                        Enabled = true
                    }
                };

                var url =
                    $"{_azureConfig.BaseUrl}/alertRules/{SCHEDULED_ALERT_RULE_NAME}?api-version={_azureConfig.ApiVersion}";

                var serialized = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                });

                var request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = new StringContent(serialized, Encoding.UTF8, "application/json")
                };
                await _authenticationService.AuthenticateRequest(request);

                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

                var error = await response.Content.ReadAsStringAsync();
                var formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error calling the API: \n" +
                                       JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: \n" + ex.Message);
            }
        }

        public async Task<string> DeleteAlertRule()
        {
            try
            {
                var url =
                    $"{_azureConfig.BaseUrl}/alertRules/{_azureConfig.LastCreatedAction}?api-version={_azureConfig.ApiVersion}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                await _authenticationService.AuthenticateRequest(request);

                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception("Not found, please create a new Alert first...");

                var error = await response.Content.ReadAsStringAsync();
                var formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error calling the API: \n" +
                                       JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: \n" + ex.Message);
            }
        }

        public async Task<string> GetFusionAlertRule()
        {
            return await GetAlertRuleByName(_azureConfig.LastCreatedAction);
        }

        public async Task<string> GetMicrosoftSecurityIdentityCreationAlertRule()
        {
            return await GetAlertRuleByName(MICROSOFT_SECURITY_INCIDENT_CREATION_RULE_NAME);
        }

        public async Task<string> GetScheduledAlertRule()
        {
            return await GetAlertRuleByName(SCHEDULED_ALERT_RULE_NAME);
        }

        public async Task<string> GetAlertRuleByName(string ruleName)
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/alertRules/{ruleName}?api-version={_azureConfig.ApiVersion}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                await _authenticationService.AuthenticateRequest(request);
                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

                var error = await response.Content.ReadAsStringAsync();
                var formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error calling the API: \n" +
                                       JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: \n" + ex.Message);
            }
        }

        public async Task<string> GetAlertRules()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/alertRules?api-version={_azureConfig.ApiVersion}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                await _authenticationService.AuthenticateRequest(request);
                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

                var error = await response.Content.ReadAsStringAsync();
                var formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error calling the API: \n" +
                                       JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: \n" + ex.Message);
            }
        }
    }
}