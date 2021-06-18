using Fortnite.Net.Exceptions;
using Fortnite.Net.Objects;

using Newtonsoft.Json;

using RestSharp;

using System.Threading;
using System.Threading.Tasks;

namespace Fortnite.Net.Services
{
    public abstract class BaseService
    {

        public abstract string BaseUrl { get; }

        internal FortniteApiClient Client { get; set; }
        internal RestClient RestClient { get; set; }

        internal BaseService(FortniteApiClient client)
        {
            Client = client;
            RestClient = client.CreateRestClient(this);
        }

        internal virtual async Task<FortniteResponse> ExecuteAsync(
            RestRequest request, 
            bool withAuth = false,
            CancellationToken token = default)
            => await ExecuteAsync<object>(request, withAuth, token, false).ConfigureAwait(false);

        internal virtual async Task<FortniteResponse<T>> ExecuteAsync<T>(
            RestRequest request,
            bool withAuth = false,
            CancellationToken token = default,
            bool withData = true,
            string accessToken = null,
            bool requiresLogin = true)
        {
            if (requiresLogin)
            {
                var isLoggedIn = await Client.VerifyTokenAsync()
                    .ConfigureAwait(false);
                if (!isLoggedIn)
                {
                    throw new FortniteException("You need to be logged in to use this!");
                }
            }

            if(withAuth)
            {
                request.AddHeader("Authorization", $"bearer {accessToken ?? Client.CurrentLogin.AccessToken}");
            }

            var response = await RestClient.ExecuteAsync(request, token)
                .ConfigureAwait(false);
            var content = response.Content;
            var fortniteResponse = new FortniteResponse<T>
            {
                HttpStatusCode = response.StatusCode
            };

            if (response.IsSuccessful && withData)
            {
                fortniteResponse.Data =
                    JsonConvert.DeserializeObject<T>(content, NewtonsoftSerializer.SerializerSettings);
            }

            if(!response.IsSuccessful)
            {
                fortniteResponse.Error =
                    JsonConvert.DeserializeObject<EpicError>(content, NewtonsoftSerializer.SerializerSettings);
            }

            return fortniteResponse;
        }

    }
}
