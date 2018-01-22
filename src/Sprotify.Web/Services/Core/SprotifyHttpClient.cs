using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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

        public Task<HttpClient> GetClient()
        {
            return Task.FromResult(_httpClient);
        }
    }
}