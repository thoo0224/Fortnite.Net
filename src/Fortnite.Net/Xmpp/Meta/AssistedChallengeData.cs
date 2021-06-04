using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class AssistedChallengeData
    {

        [JsonProperty("questItemDef")]
        public string QuestItemDefinition => "None";

        [JsonProperty("objectivesCompleted")]
        public int ObjectivesCompleted => 0;

    }
}
