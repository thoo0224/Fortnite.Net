using Fortnite.Net.Xmpp;

using Newtonsoft.Json;

using System.Collections.Generic;

namespace Fortnite.Net.Objects.Party
{
    public class PartyCreation
    {

        [JsonProperty("config")]
        public Dictionary<string, object> Config { get; set; }

        [JsonProperty("join_info")]
        public PartyJoinInfo JoinInfo { get; set; }

        [JsonProperty("meta")]
        public Dictionary<string, string> Meta { get; set; }

        public PartyCreation(XmppClient xmppClient)
        {
            JoinInfo = new PartyJoinInfo(xmppClient, false);
            Config = new Dictionary<string, object>()
            {
                { "join_confirmation", false },
                { "joinability", "OPEN" },
                { "max_size", 16 }
            };
            Meta = new Dictionary<string, string>()
            {
                { "urn:epic:cfg:party-type-id_s", "default" },
                { "urn:epic:cfg:build-id_s", "1:3:" },
                { "urn:epic:cfg:join-request-action_s", "Manual" },
                { "urn:epic:cfg:presence-perm_s", "Noone" },
                { "urn:epic:cfg:invite-perm_s", "Noone" },
                { "urn:epic:cfg:chat-enabled_b", "true" },
                { "urn:epic:cfg:accepting-members_b", "false" },
                { "urn:epic:cfg:not-accepting-members-reason_i", "0" }
            };
        }

    }
}
