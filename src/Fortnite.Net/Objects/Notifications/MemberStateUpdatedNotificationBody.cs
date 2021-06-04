using System;
using Newtonsoft.Json;

using System.Collections.Generic;

namespace Fortnite.Net.Objects.Notifications
{
    public class MemberStateUpdatedNotificationBody
    {

        public int Revision { get; set; }

        public string Ns { get; set; }

        [JsonProperty("party_id")]
        public string PartyId { get; set; }
        
        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("member_state_removed")]
        public List<string> MemberStateRemoved { get; set; }

        [JsonProperty("member_state_updated")]
        public Dictionary<string, object> MemberStateUpdated { get; set; }

        [JsonProperty("joined_at")]
        public DateTime JoinedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

    }
}
