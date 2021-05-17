using Fortnite.Net.Exceptions;
using Fortnite.Net.Xmpp;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Fortnite.Net.Objects.Party
{
    public class PartyInvitation
    {

        [JsonIgnore]
        internal XmppClient Client { get; set; }

        [JsonProperty("party_id")]
        public string PartyId { get; set; }

        [JsonProperty("sent_by")]
        public string SentBy { get; set; }

        public Dictionary<string, string> Meta { get; set; }

        [JsonProperty("sent_to")]
        public string SentTo { get; set; }

        [JsonProperty("sent_at")]
        public DateTime SentAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }

        public string Status { get; set; }

        [JsonIgnore]
        public bool ExpiredOrDeclined { get; set; }

        public async Task AcceptAsync()
        {
            if (DateTime.UtcNow > ExpiresAt)
            {
                ExpiredOrDeclined = true;
            }

            VerifyParty();

            if (Client.CurrentParty != null)
            {
                throw new FortniteException("Already in a party.");
            }

            await Client.Client.PartyService.JoinPartyAsync(PartyId);
            await Client.Client.PartyService.DeletePingAsync(SentBy);
            ExpiredOrDeclined = true;
        }

        private void VerifyParty()
        {
            if (ExpiredOrDeclined)
            {
                throw new FortniteException($"Party invitation {PartyId} has expires or was declined!");
            }
        }

    }
}
