using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sprotify.Web.Services.Core
{
    public abstract class ApiServiceBase
    {
        private readonly SprotifyHttpClient _sprotifyclient;

        protected ApiServiceBase(SprotifyHttpClient sprotifyclient)
        {
            _sprotifyclient = sprotifyclient;
        }

        protected async Task<T> Get<T>(string resource)
        {
            var client = await _sprotifyclient.GetClient().ConfigureAwait(false);
            var response = await client.GetAsync(resource).ConfigureAwait(false);

            await ThrowIfUnsuccessful(response).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected async Task<T> Post<T>(string resource, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var client = await _sprotifyclient.GetClient().ConfigureAwait(false);
            var response = await client.PostAsync(resource, content).ConfigureAwait(false);

            await ThrowIfUnsuccessful(response).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected async Task<T> Put<T>(string resource, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var client = await _sprotifyclient.GetClient().ConfigureAwait(false);
            var response = await client.PutAsync(resource, content).ConfigureAwait(false);

            await ThrowIfUnsuccessful(response).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected async Task Delete(string resource)
        {
            var client = await _sprotifyclient.GetClient().ConfigureAwait(false);
            var response = await client.DeleteAsync(resource).ConfigureAwait(false);

            await ThrowIfUnsuccessful(response).ConfigureAwait(false);
        }

        private static async Task ThrowIfUnsuccessful(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new ResourceNotFoundException();
                    case HttpStatusCode.BadRequest:
                        var badRequestData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new BadRequestException(badRequestData);
                    default:
                        var errData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new HttpException(response.StatusCode, errData);
                }
            }
        }
    }
}