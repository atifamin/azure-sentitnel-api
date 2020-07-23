using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzureSentinel_ManagementAPI.DataConnectors.Models;
using AzureSentinel_ManagementAPI.Infrastructure.Authentication;
using AzureSentinel_ManagementAPI.Infrastructure.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSentinel_ManagementAPI.DataConnectors
{
    public class DataConnectorsController
    {
        private readonly AzureSentinelApiConfiguration _azureConfig;
        private readonly AuthenticationService _authenticationService;

        public DataConnectorsController(
            AzureSentinelApiConfiguration azureConfig,
            AuthenticationService authenticationService
        )
        {
            _azureConfig = azureConfig;
            _authenticationService = authenticationService;
        }

        public async Task<string> GetDataConnectors()
        {
            _azureConfig.LastCreatedDataConnector = Guid.NewGuid().ToString();
            
            try
            {
                var url = $"{_azureConfig.BaseUrl}/dataConnectors?api-version={_azureConfig.ApiVersion}";
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
        
        public async Task<string> CreateDataConnector()
        {
            try
            {
                var payload = new ASCDataConnectorPayload
                {
                    Kind = DataConnectorKind.AzureSecurityCenter,
                    PropertiesPayload = new ASCDataConnectorPropertiesPayload
                    {
                        SubscriptionId = $"{_azureConfig.SubscriptionId}",
                        DataTypesPayload = new ASCDataConnectorDataTypesPayload
                        {
                            Alerts = new DataTypeConnectionStatePayload
                            {
                                State = DataConnectionState.Disabled
                            }
                        }
                    }
                };

                _azureConfig.LastCreatedDataConnector = Guid.NewGuid().ToString();

                var url =
                    $"{_azureConfig.BaseUrl}/dataConnectors/{_azureConfig.LastCreatedDataConnector}?api-version={_azureConfig.ApiVersion}";

                var serialized = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
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
        
        public async Task<string> DeleteDataConnector()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/dataConnectors/{_azureConfig.LastCreatedDataConnector}?api-version={_azureConfig.ApiVersion}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                await _authenticationService.AuthenticateRequest(request);

                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception("Not found, please create a new DataConnector first...");

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