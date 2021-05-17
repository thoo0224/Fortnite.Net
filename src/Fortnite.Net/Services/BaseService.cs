﻿using Fortnite.Net.Objects;
using Fortnite.Net.Objects.Epic;

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

        internal virtual async Task<FortniteResponse> ExecuteAsync(RestRequest request, bool withAuth = false,
            CancellationToken token = default)
            => await ExecuteAsync<object>(request, withAuth, token).ConfigureAwait(false);

        internal virtual async Task<FortniteResponse<T>> ExecuteAsync<T>(RestRequest request, bool withAuth = false, 
            CancellationToken token = default)
        {
            if(withAuth)
            {
                request.AddHeader("Authorization", $"bearer {Client.CurrentLogin.AccessToken}");
            }

            var response = await RestClient.ExecuteAsync(request, token)
                .ConfigureAwait(false);
            var content = response.Content;
            var fortniteResponse = new FortniteResponse<T>
            {
                HttpStatusCode = response.StatusCode
            };

            if (response.IsSuccessful)
            {
                fortniteResponse.Data =
                    JsonConvert.DeserializeObject<T>(content, NewtonsoftSerializer.SerializerSettings);
            } else
            {
                fortniteResponse.Error =
                    JsonConvert.DeserializeObject<EpicError>(content, NewtonsoftSerializer.SerializerSettings);
            }

            return fortniteResponse;
        }

    }
}
