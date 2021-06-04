using Fortnite.Net.Xmpp;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fortnite.Net.Xmpp.Meta;
using Fortnite.Net.Xmpp.Payloads;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Fortnite.Net.Objects.Party
{
    public class Party
    {

        [JsonIgnore]
        internal XmppClient Client { get; set; }

        public string Id { get; set; }

        [J("created_at")]
        public DateTime CreatedTime { get; set; }

        [J("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public Dictionary<string, object> Config { get; set; }

        public List<PartyInvitation> Invites { get; set; }

        public List<PartyMember> Members { get; set; } = new List<PartyMember>();

        public List<PartyMember> Applicants { get; set; }

        public int Revision { get; set; }

        public object[] Intentions { get; set; }

        public Dictionary<string, string> Meta { get; set; }

        public async Task UpdatePresence(XmppClient client)
        {
            var presence = new Presence
            {
                Status = $"Battle Royale Lobby - {Members.Count} / {Config["max_size"]} in Party",
                Properties = new Dictionary<string, object>
                {
                    { "FortBasicInfo_j", new FortBasicInfo()},
                    { "FortGameplayStats_j", new FortGameplayStats()},
                    { "FortLFG_I", "0"},
                    { "FortPartySize_i", 1},
                    { "FortSubGame_i", 1},
                    { "InUnjoinableMatch_b", false},
                    { "party.joininfodata.286331153_j", new
                    {
                        bIsPrivate = ""
                    }}
                }
            };

            await client.SendPresenceAsync(presence);
        }

    }
}
