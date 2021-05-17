using Fortnite.Net.Objects.Party;

namespace Fortnite.Net.Xmpp.EventArgs
{
    public class GroupChatMessageReceivedEventArgs : System.EventArgs
    {

        public Party Party { get; set; }
        public PartyMember From { get; set; }

    }
}
