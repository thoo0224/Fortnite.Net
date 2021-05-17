using Fortnite.Net.Xmpp;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<PartyMember> Members { get; set; }

        public List<PartyMember> Applicants { get; set; }

        public int Revision { get; set; }

        public object[] Intentions { get; set; }

        public Dictionary<string, string> Meta { get; set; }

        [JsonIgnore] public PartyMember Leader => Members.FirstOrDefault(x => x.Role.Equals("CAPTAIN"));

    }
}
