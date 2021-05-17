using Fortnite.Net.Objects.Party;

namespace Fortnite.Net.Xmpp.EventArgs
{
    public class PartyChatMessageReceivedEventArgs : System.EventArgs
    {

        public Party Party { get; set; }
        public PartyMember From { get; set; }

    }
}
