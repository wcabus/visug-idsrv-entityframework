using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Sprotify.Web.Services.Core
{
    public class SprotifyHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public SprotifyHttpClient(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetValue<string>("ApiBaseUri")),
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpClient> GetClient()
        {
            string accessToken;

            var currentContext = _httpContextAccessor.HttpContext;
            
            // should we renew access & refresh tokens?
            // get expires_at value
            var expiresAt = await currentContext.GetTokenAsync("expires_at").ConfigureAwait(false);

            // compare - make sure to use the exact date formats for comparison 
            // (UTC, in this case)
            if (string.IsNullOrWhiteSpace(expiresAt)
                || ((DateTime.Parse(expiresAt).AddSeconds(-60)).ToUniversalTime()
                < DateTime.UtcNow))
            {
                accessToken = await RenewTokens().ConfigureAwait(false);
            }
            else
            {
                // get access token
                accessToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).ConfigureAwait(false);
            }

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                _httpClient.SetBearerToken(accessToken);
            }
            
            return _httpClient;
        }

        private async Task<string> RenewTokens()
        {
            // get the current HttpContext to access the tokens
            var currentContext = _httpContextAccessor.HttpContext;

            // get the metadata
            var discoveryClient = new DiscoveryClient(Startup.Configuration.GetValue<string>("Authority"));
            var metaDataResponse = await discoveryClient.GetAsync().ConfigureAwait(false);

            // create a new token client to get new tokens
            var tokenClient = new TokenClient(metaDataResponse.TokenEndpoint,
                Startup.Configuration.GetValue<string>("ClientId"), Startup.Configuration.GetValue<string>("ClientSecret"));

            // get the saved refresh token
            var currentRefreshToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).ConfigureAwait(false);

            if (currentRefreshToken == null)
            {
                return null;
            }

            // refresh the tokens
            var tokenResult = await tokenClient.RequestRefreshTokenAsync(currentRefreshToken).ConfigureAwait(false);

            if (!tokenResult.IsError)
            {
                // Save the tokens. 

                // get auth info
                var authenticateInfo = await currentContext.AuthenticateAsync("Cookies").ConfigureAwait(false);

                // create a new value for expires_at, and save it
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                authenticateInfo.Properties.UpdateTokenValue("expires_at", expiresAt.ToString("o", CultureInfo.InvariantCulture));

                authenticateInfo.Properties.UpdateTokenValue(
                    OpenIdConnectParameterNames.AccessToken,
                    tokenResult.AccessToken);
                authenticateInfo.Properties.UpdateTokenValue(
                    OpenIdConnectParameterNames.RefreshToken,
                    tokenResult.RefreshToken);

                // we're signing in again with the new values.  
                await currentContext.SignInAsync("Cookies", authenticateInfo.Principal, authenticateInfo.Properties).ConfigureAwait(false);

                // return the new access token 
                return tokenResult.AccessToken;
            }

            throw new Exception("Problem encountered while refreshing tokens.",
                tokenResult.Exception);
        }
    }
}