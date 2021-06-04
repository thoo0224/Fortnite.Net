using Fortnite.Net.Objects.Party;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Fortnite.Net.Objects.Notifications
{
    public class MemberJoinedNotificationBody : BaseNotificationBody
    {

        public PartyMemberConnection Connection { get; set; }
        
        public int Revision { get; set; }

        public string Ns { get; set; }

        [JsonProperty("party_id")]
        public string PartyId { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("member_state_updated")]
        public Dictionary<string, string> MemberStateUpdated { get; set; }

        [JsonProperty("joined_at")]
        public DateTime JoinedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

    }
}
