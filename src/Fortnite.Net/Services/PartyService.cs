using Fortnite.Net.Objects;
using Fortnite.Net.Objects.Party;

using RestSharp;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fortnite.Net.Services
{
    public class PartyService : BaseService
    {

        public override string BaseUrl { get; } = "https://party-service-prod.ol.epicgames.com/party/api/v1/Fortnite";

        public PartyService(FortniteApiClient client)
            : base(client)
        {
        }

        /// <summary>
        /// Gets all the party pings of the pinger
        /// </summary>
        /// <param name="pingerId">Id of the pinger</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of all the parties</returns>
        public async Task<FortniteResponse<List<Party>>> GetPingsAsync(
            string pingerId,
            CancellationToken cancellationToken = default)
        {
            var request = new RestRequest($"/user/{Client.CurrentLogin.AccountId}/pings/{pingerId}/parties");

            var response = await ExecuteAsync<List<Party>>(request, true, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Delete all the pings of the pinger
        /// </summary>
        /// <param name="pingerId">Id of the pinger</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<FortniteResponse> DeletePingAsync(
            string pingerId,
            CancellationToken cancellationToken = default)
        {
            var request = new RestRequest($"/user/{Client.CurrentLogin.AccountId}/pings/{pingerId}", Method.DELETE);

            var response = await ExecuteAsync<List<Party>>(request, true, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Joins the party and the party chat
        /// </summary>
        /// <param name="partyId">Id of the party</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<FortniteResponse> JoinPartyAsync(
            string partyId,
            CancellationToken cancellationToken = default)
        {
            var body = new PartyJoinInfo(Client.XmppClient);

            var request = new RestRequest($"/parties/{partyId}/members/{Client.CurrentLogin.AccountId}/join",
                Method.POST);
            request.AddJsonBody(body);

            var response = await ExecuteAsync(request, true, cancellationToken)
                .ConfigureAwait(false);

            var partyResponse = await GetPartyAsync(partyId, cancellationToken)
                .ConfigureAwait(false);
            Client.XmppClient.CurrentParty = partyResponse.Data;
            await Client.XmppClient.JoinPartyChatAsync();
            return response;
        }

        /// <summary>
        /// Gets the party
        /// </summary>
        /// <param name="partyId">Id of the party</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The party</returns>
        public async Task<FortniteResponse<Party>> GetPartyAsync(
            string partyId,
            CancellationToken cancellationToken = default)
        {
            var request = new RestRequest($"/parties/{partyId}");

            var response = await ExecuteAsync<Party>(request, true, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

    }
}
