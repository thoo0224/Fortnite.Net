using System;
using System.Threading.Tasks;
using Fortnite.Net.Utils;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Fortnite.Net.Services
{
    public abstract class EpicService
    {

        internal readonly FortniteApi _api;
        internal readonly RestClient _restClient;

        internal EpicService(FortniteApi api, string baseUrl)
        {
            _api = api;
            _restClient = new RestClient(baseUrl);
            _restClient.UseSerializer<JsonNetSerializer>();
        }
        
        internal async Task<T> SendBaseAsync<T>(
            string resource,
            Method method = Method.GET,
            bool authorization = true, 
            Action<RestRequest> requestAction = null)
        {
            var request = new RestRequest(resource, method);
            requestAction?.Invoke(request);
            if (authorization)
            {
                request.AddHeader("Authorization", $"bearer {_api.LoginModel.AccessToken}");
            }
            var response = await _restClient.HandleRequest<T>(request);
            return response;
        }

    }
}