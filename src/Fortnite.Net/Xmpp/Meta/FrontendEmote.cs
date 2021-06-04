using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class FrontendEmote
    {

        [JsonProperty("FrontendEmote")]
        public FrontendEmoteData Data { get; set; }

        public FrontendEmote(string id, bool isEmoji = false, int section = -1)
        {
            Data = new FrontendEmoteData
            {
                ItemDefinition = id == null ? "None" :
                    isEmoji ? $"/Game/Athena/Items/Cosmetics/Dances/Emoji/{id}.{id}" :
                    $"/Game/Athena/Items/Cosmetics/Dances/{id}.{id}",
                EmoteSection = section
            };
        }

    }
}
