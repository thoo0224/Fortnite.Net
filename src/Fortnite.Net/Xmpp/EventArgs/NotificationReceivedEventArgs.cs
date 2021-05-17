using System.Xml;

namespace Fortnite.Net.Xmpp.EventArgs
{
    public class NotificationReceivedEventArgs : System.EventArgs
    {

        public string Type { get; set; }
        public object Body { get; set; }

    }
}
