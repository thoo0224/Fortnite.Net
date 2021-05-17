using System.Xml;

namespace Fortnite.Net.Xmpp.EventArgs
{
    public class MessageReceivedEventArgs : System.EventArgs
    {

        public string Raw { get; set; }
        public XmlDocument Document { get; set; }

    }
}
