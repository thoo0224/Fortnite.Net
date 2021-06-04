using System.Collections.Generic;

using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Fortnite.Net.Xmpp.Payloads
{
    public class Presence
    {

        public string Status { get; set; }
        public string SessionId { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        [J("bIsPlaying")] public bool IsPlaying { get; set; }
        [J("bIsJoinable")] public bool IsJoinable { get; set; }
        [J("bHasVoiceSupport")] public bool HasVoiceSupport { get; set; }

    }
}
