using System;
using Newtonsoft.Json;

namespace Fortnite.Net.Objects.Notifications
{
    public class PingNotificationBody : BaseNotificationBody
    {

        public string Ns { get; set; }
        public DateTime Expires { get; set; }

        [JsonProperty("pinger_id")] 
        public string PingerId { get; set; }

        [JsonProperty("pinger_dn")] 
        public string PingerDisplayName { get; set; }

    }
}
