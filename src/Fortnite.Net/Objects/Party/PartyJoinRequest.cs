using System.Collections.Generic;
using Fortnite.Net.Xmpp;

namespace Fortnite.Net.Objects.Party
{
    public class PartyJoinRequest
    {

        public List<PartyJoinUser> Users { get; set; }

        public PartyJoinRequest(XmppClient client)
        {
            Users = new List<PartyJoinUser>()
            {
                new PartyJoinUser
                {
                    Id = client.Client.CurrentLogin.AccountId,
                    DisplayName = client.Client.CurrentLogin.DisplayName,
                    Platform = client.Platform.ToString().ToUpper(),
                    Data = new Dictionary<string, string>
                    {
                        {"CrossplayReference", "1"},
                        {"SubGame_u", "1"}
                    }
                }
            };
        }

    }
}
