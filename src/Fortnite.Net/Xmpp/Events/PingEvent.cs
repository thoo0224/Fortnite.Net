using System;
using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Events
{
    public class PingEvent : BaseEvent
    {

        public string Ns { get; set; }
        [JsonProperty("pinger_id")] public string PingerId { get; set; }
        [JsonProperty("pinger_dn")] public string PingerDisplayName { get; set; }
        public DateTime Expires { get; set; }

    }
}
