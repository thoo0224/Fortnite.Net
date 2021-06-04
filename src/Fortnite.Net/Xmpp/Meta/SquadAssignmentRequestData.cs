using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class SquadAssignmentRequestData
    {

        [JsonProperty("startingAbsoluteIdx")]
        public int StartingAbsoluteIndex => -1;

        [JsonProperty("targetAbsoluteIdx")]
        public int TargetAbsoluteIdx => -1;

        [JsonProperty("swapTargetMemberId")]
        public string SwapTargetMemberId => "INVALID";

        [JsonProperty("version")]
        public int Version => 0;

    }
}
