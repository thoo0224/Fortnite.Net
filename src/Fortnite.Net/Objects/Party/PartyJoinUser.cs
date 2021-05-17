using Newtonsoft.Json;

using System.Collections.Generic;

namespace Fortnite.Net.Objects.Party
{
    public class PartyJoinUser
    {

        public string Id { get; set; }

        [JsonProperty("dn")]
        public string DisplayName { get; set; }

        [JsonProperty("plat")]
        public string Platform { get; set; }

        public Dictionary<string, string> Data { get; set; }

    }
}
