using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AzureSentinel_ManagementAPI.Infrastructure.Authentication;
using AzureSentinel_ManagementAPI.Infrastructure.Configuration;
using Newtonsoft.Json;

namespace AzureSentinel_ManagementAPI.AlertRuleTemplates
{
    public class AlertRuleTemplatesController
    {
        private const string RULE_TEMPLATE_NAME = "157c0cfc-d76d-463b-8755-c781608cdc1a";
        
        private readonly AuthenticationService _authenticationService;
        private readonly AzureSentinelApiConfiguration _azureConfig;

        public AlertRuleTemplatesController(AuthenticationService authenticationService, AzureSentinelApiConfiguration azureConfig)
        {
            _authenticationService = authenticationService;
            _azureConfig = azureConfig;
        }

        public async Task<string> GetAlertRuleTemplates()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/alertRuleTemplates?api-version={_azureConfig.ApiVersion}";
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

        public async Task<string> GetAlertRuleTemplateById(string templateId)
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/alertRuleTemplates/{templateId}?api-version={_azureConfig.ApiVersion}";
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