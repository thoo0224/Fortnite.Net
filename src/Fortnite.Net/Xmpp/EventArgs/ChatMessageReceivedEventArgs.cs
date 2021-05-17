namespace Fortnite.Net.Xmpp.EventArgs
{
    public class ChatMessageReceivedEventArgs : System.EventArgs
    {

        public string From { get; set; }
        public string Message { get; set; }

    }
}
