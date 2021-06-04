using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class LobbyStateData
    {

        [JsonProperty("inGameReadyCheckStatus")]
        public string InGameReadyCheckStatus => "None";

        [JsonProperty("gameReadiness")]
        public string GameReadiness => "NotReady";

        [JsonProperty("readyInputType")]
        public string ReadyInputType => "Count";

        [JsonProperty("currentInputType")]
        public string CurrentInputType => "MouseAndKeyboard";

        [JsonProperty("hiddenMatchmakingDelayMax")]
        public int HiddenMatchmakingDelayMax => 0;

        [JsonProperty("hasPreloadedAthena")]
        public bool HasPreloadedAthena => false;

    }
}
