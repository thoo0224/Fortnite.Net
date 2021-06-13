﻿using RestSharp;

using System.Threading.Tasks;

namespace Fortnite.Net.Services
{
    public class FortniteService : BaseService
    {

        public override string BaseUrl => "https://fortnite-public-service-prod11.ol.epicgames.com";

        internal FortniteService(FortniteApiClient client) : base(client)
        {
        }

        // This will be better soon. (there's going to be a better system for this anyway)
        public async Task ExecuteClientCommandAsync(string command, string profileId, object payload)
        {
            var request = new RestRequest($"/fortnite/api/game/v2/profile/{Client.CurrentLogin.AccountId}/client/{command}", Method.POST);
            request.AddQueryParameter("profileId", profileId);
            request.AddJsonBody(payload);

            await ExecuteAsync(request);
        }

    }
}
