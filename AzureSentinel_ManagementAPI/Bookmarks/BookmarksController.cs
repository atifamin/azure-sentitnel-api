using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzureSentinel_ManagementAPI.Bookmarks.Models;
using AzureSentinel_ManagementAPI.Infrastructure.Authentication;
using AzureSentinel_ManagementAPI.Infrastructure.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSentinel_ManagementAPI.Bookmarks
{
    public class BookmarksController
    {
        private readonly AzureSentinelApiConfiguration _azureConfig;
        private readonly AuthenticationService _authenticationService;

        public BookmarksController(
            AzureSentinelApiConfiguration azureConfig,
            AuthenticationService authenticationService)
        {
            _azureConfig = azureConfig;
            _authenticationService = authenticationService;
        }

        public async Task<string> CreateBookmark()
        {
            _azureConfig.LastCreatedBookmark = Guid.NewGuid().ToString();
            
            try
            {
                var payload = new BookmarkPayload
                {
                    PropertiesPayload = new BookmarkPropertiesPayload
                    {
                       Query = "SecurityEvent",
                       DisplayName = $"Incident: {_azureConfig.LastCreatedBookmark}",
                       Labels = new List<string>{"Tag1", "Tag2"},
                       QueryResult = "Security Event query result"
                    }
                };

                var url = $"{_azureConfig.BaseUrl}/bookmarks/{_azureConfig.LastCreatedBookmark}?api-version={_azureConfig.ApiVersion}";

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

        public async Task<string> DeleteBookmark()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/bookmarks/{_azureConfig.LastCreatedBookmark}?api-version={_azureConfig.ApiVersion}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                await _authenticationService.AuthenticateRequest(request);

                var http = new HttpClient();
                var response = await http.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception("Not found, please create a new Bookmark first...");
                
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

        public async Task<string> GetBookmarkById()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/bookmarks/{_azureConfig.LastCreatedBookmark}?api-version={_azureConfig.ApiVersion}";
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

        public async Task<string> GetBookmarks()
        {
            try
            {
                var url = $"{_azureConfig.BaseUrl}/bookmarks?api-version={_azureConfig.ApiVersion}";
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