using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzureSentinel_ManagementAPI.Actions.Models;
using AzureSentinel_ManagementAPI.Infrastructure.Authentication;
using AzureSentinel_ManagementAPI.Infrastructure.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSentinel_ManagementAPI.Actions
{
    public class ActionsController
    {
        private const string ALERT_RULE_NAME = "fusion-rule-3";
        private const string TRIGGER_URI = "https://management.azure.com/subscriptions/e7e57952-104e-4ed8-a360-3da35efdd32a/resourceGroups/myRg/providers/Microsoft.Logic/workflows/myLogicApp";

        private readonly AzureSentinelApiConfiguration _azureConfig;
        private readonly AuthenticationService _authenticationService;

        public ActionsController(
            AzureSentinelApiConfiguration azureConfig,
            AuthenticationService authenticationService)
        {
            _azureConfig = azureConfig;
            _authenticationService = authenticationService;
        }

        public async Task<string> CreateAction(string alertId)
        {
            try
            {
                var payload = new ActionRequestPayload
                {
                    PropertiesPayload = new ActionRequestPropertiesPayload
                    {
                        LogicAppResourceId = _azureConfig.WorkflowId,
                        TriggerUri = TRIGGER_URI
                    }
                };

                _azureConfig.LastCreatedAction = Guid.NewGuid().ToString();
                var url = $"{_azureConfig.BaseUrl}/alertRules/{alertId}/actions/{_azureConfig.LastCreatedAction}?api-version={_azureConfig.ApiVersion}";

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

        public async Task<string> DeleteAction(string alertId)
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/alertRules/{alertId}/actions/{_azureConfig.LastCreatedAction}?api-version={_azureConfig.ApiVersion}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                await _authenticationService.AuthenticateRequest(request);

                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception("Not found, please create a new Action first...");
                
                
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

        public async Task<string> GetActionById(string alertId)
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/alertRules/{alertId}/actions/{_azureConfig.LastCreatedAction}?api-version={_azureConfig.ApiVersion}";
               
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

        public async Task<string> GetActionsByRule(string alertId)
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/alertRules/{alertId}/actions?api-version={_azureConfig.ApiVersion}";

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