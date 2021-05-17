using System;
using System.Collections.Generic;

using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Fortnite.Net.Objects.Party
{
    public class PartyMemberConnection
    {

        public string Id { get; set; }

        [J("connected_at")] public DateTime ConnectedAt { get; set; }

        [J("updated_at")] public DateTime UpdatedAt { get; set; }

        [J("yield_leadership")]
        public bool YieldLeadership { get; set; }

        public Dictionary<string, string> Meta { get; set; }

    }
}
