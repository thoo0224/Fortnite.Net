using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class FrontendEmoteData
    {

        [JsonProperty("emoteItemDef")]
        public string ItemDefinition { get; set; }

        [JsonProperty("emoteEKey")] 
        public string EmoteKey { get; set; } = "";

        public int EmoteSection { get; set; }

    }
}
