using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class SquadAssignmentRequest
    {

        [JsonProperty("MemberSquadAssignmentRequest")]
        public SquadAssignmentRequestData Data { get; set; }

        public SquadAssignmentRequest()
        {
            Data = new SquadAssignmentRequestData();
        }

    }
}
