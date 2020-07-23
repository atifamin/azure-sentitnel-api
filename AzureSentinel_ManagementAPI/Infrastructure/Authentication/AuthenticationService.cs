using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AzureSentinel_ManagementAPI.Infrastructure.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureSentinel_ManagementAPI.Infrastructure.Authentication
{
    //Access token class to authenticate and obtain AAD Token for future calls
    public class AuthenticationService
    {
        private readonly ClientCredential _credential;
        private readonly AuthenticationContext _authContext;
        private readonly AzureSentinelApiConfiguration _azureConfig;

        public AuthenticationService(AzureSentinelApiConfiguration azureConfig)
        {
            _azureConfig = azureConfig;

            _authContext = new AuthenticationContext("https://login.microsoftonline.com/" + _azureConfig.TenantId);
            _credential = new ClientCredential(_azureConfig.AppId, _azureConfig.AppSecret);
        }

        public async Task<AuthenticationResult> GetToken()
        {
            try
            {
                return
                    await _authContext.AcquireTokenAsync("https://management.azure.com", _credential);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Acquiring Access Token: \n" + ex.Message);
            }
        }

        public async Task AuthenticateRequest(HttpRequestMessage request)
        {
            var token = await GetToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }
    }
}