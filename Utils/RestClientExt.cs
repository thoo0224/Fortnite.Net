using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fortnite.Net.Exceptions;
using Fortnite.Net.Model;
using Newtonsoft.Json;
using RestSharp;

namespace Fortnite.Net.Utils
{
    public static class RestClientExt
    {

        public static async Task<T> HandleRequest<T>(this RestClient client, RestRequest request)
        {
            var response = await client.ExecuteAsync<T>(request);
            if (response.StatusCode == HttpStatusCode.OK) return response.Data;
            var raw = Encoding.UTF8.GetString(response.RawBytes);
            var error = JsonConvert.DeserializeObject<EpicError>(raw);
            throw new EpicException(error);
        }
        
        /*public static async Task HandleRequestNoType(this RestClient client, RestRequest request)
        {
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == HttpStatusCode.OK) return;
            var raw = Encoding.UTF8.GetString(response.RawBytes);
            var error = JsonConvert.DeserializeObject<EpicError>(raw);
            throw new EpicException(error);
        }*/
        
    }
}