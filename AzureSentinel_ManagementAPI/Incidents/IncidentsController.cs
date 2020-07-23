using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzureSentinel_ManagementAPI.Incidents.Models;
using AzureSentinel_ManagementAPI.Incidents.Models.Comments;
using AzureSentinel_ManagementAPI.Infrastructure.Authentication;
using AzureSentinel_ManagementAPI.Infrastructure.Configuration;
using AzureSentinel_ManagementAPI.Infrastructure.SharedModels.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSentinel_ManagementAPI.Incidents
{
    public class IncidentsController
    {
        private const string INCIDENT_NAME = "incident-5";
        private const string INCIDENT_COMMENT_NAME = "incident-comment-1";

        private readonly AzureSentinelApiConfiguration _azureConfig;
        private readonly AuthenticationService _authenticationService;

        public IncidentsController(
            AzureSentinelApiConfiguration azureConfig,
            AuthenticationService authenticationService)
        {
            _azureConfig = azureConfig;
            _authenticationService = authenticationService;
        }

        public async Task<string> CreateIncident(IncidentPayload payload, string incidentId)
        {
            try
            {
                //var payload = new IncidentPayload
                //{
                //    PropertiesPayload = new IncidentPropertiesPayload
                //    {
                //        Severity = Severity.Low,
                //        Status = IncidentStatus.New,
                //        Title = INCIDENT_NAME
                //    }
                //};

                var url = $"{_azureConfig.BaseUrl}/incidents/{incidentId}?api-version={_azureConfig.ApiVersion}";

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

        public async Task<string> DeleteIncident()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/incidents/{INCIDENT_NAME}?api-version={_azureConfig.ApiVersion}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                await _authenticationService.AuthenticateRequest(request);

                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception("Not found, please create a new Incident first...");

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

        public async Task<string> GetIncidentById()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/incidents/{INCIDENT_NAME}?api-version={_azureConfig.ApiVersion}";
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

        public async Task<string> GetIncidents()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/incidents?api-version={_azureConfig.ApiVersion}";
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

        public async Task<string> CreateIncidentComment()
        {
            try
            {
                var payload = new IncidentCommentPayload
                {
                    PropertiesPayload = new IncidentCommentPropertiesPayload
                    {
                        Message = "Just a simple message"
                    }
                };

                var url = $"{_azureConfig.BaseUrl}/incidents/{INCIDENT_NAME}/comments/{INCIDENT_COMMENT_NAME}?api-version={_azureConfig.ApiVersion}";

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

        public async Task<string> GetIncidentCommentById()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/incidents/{INCIDENT_NAME}/comments/{INCIDENT_COMMENT_NAME}?api-version={_azureConfig.ApiVersion}";
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

        public async Task<string> GetAllIncidentComments()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/incidents/{INCIDENT_NAME}/comments?api-version={_azureConfig.ApiVersion}";
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