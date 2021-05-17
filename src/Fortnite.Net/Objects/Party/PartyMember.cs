using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Fortnite.Net.Objects.Party
{
    public class PartyMember
    {

        public string Id { get; set; }

        public Dictionary<string, string> Meta { get; set; }

        public List<PartyMemberConnection> Connections { get; set; }

        public int Revision { get; set; }

        [J("updated_at")] public DateTime UpdatedAt { get; set; }

        [J("joined_at")] public DateTime JoinedAt { get; set; }

        public string Role { get; set; }

        [JsonIgnore] public bool IsPartyLeader => Role.Equals("CAPTAIN");

    }
}
