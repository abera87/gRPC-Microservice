using System.Net.Http;
using Microsoft.Extensions.Configuration;
using RMClient.Configuration;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System;

namespace RMClient
{
    public class IdentityService
    {
        private readonly HttpClient httpClient;
        private readonly IConfigurationRoot config;

        public IdentityService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            config = AppSettings.InitConfig();
        }

        public async Task<string> GetAccessTockenAsync()
        {
            var identityServerAddress = config["IdentityServer:URL"];
            try
            {
                var disco = await HttpClientDiscoveryExtensions.GetDiscoveryDocumentAsync(httpClient, identityServerAddress);

                if (disco.IsError)
                {
                    throw new ApplicationException($"Status code: {disco.IsError}, Error: {disco.Error}");
                }

                var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(httpClient, new ClientCredentialsTokenRequest
                {
                    Scope = config["ClientIdentity:Scope"],
                    ClientSecret = config["ClientIdentity:ClientSecret"],
                    Address = disco.TokenEndpoint,
                    ClientId = config["ClientIdentity:ClientId"]
                });

                if (tokenResponse.IsError)
                {
                    throw new ApplicationException($"Status code: {tokenResponse.IsError}, Error: {tokenResponse.Error}");
                }

                return tokenResponse.AccessToken;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }
        }
    }
}