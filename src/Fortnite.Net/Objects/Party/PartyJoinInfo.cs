using System.Collections.Generic;
using Fortnite.Net.Xmpp;

namespace Fortnite.Net.Objects.Party
{
    public class PartyJoinInfo
    {

        public PartyMemberConnection Connection { get; set; }

        public Dictionary<string, string> Meta { get; set; }

        public PartyJoinInfo(XmppClient client, bool isJoining = true)
        {
            Connection = new PartyMemberConnection
            {
                Id = $"{client.Jid}/{client.Resource}",
                Meta = new Dictionary<string, string>
                {
                    {"urn:epic:conn:platform_s", "WIN"},
                    {"urn:epic:conn:type_s", "game"}
                }
            };

            Meta = new Dictionary<string, string>
            {
                { "urn:epic:member:dn_s", client.Client.CurrentLogin.DisplayName }
            };

            if(isJoining)
            {
                Connection.YieldLeadership = false;
                Meta.Add("urn:epic:member:joinrequestusers_j", new PartyJoinRequest(client).ToString());
            }
        }

    }
}
