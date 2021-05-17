using Fortnite.Net.Enums;
using Fortnite.Net.Objects.Party;
using Fortnite.Net.Xmpp.EventArgs;
using Fortnite.Net.Xmpp.Events;
using Fortnite.Net.Xmpp.Payloads;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Fortnite.Net.Xmpp
{
    public class XmppClient : IAsyncDisposable
    {

        internal static XmlWriterSettings WriterSettings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            Async = true
        };

        public string Jid { get; set; }
        public string Resource { get; set; }

        public XmppNotificationHandler NotificationHandler { get; set; }
        public ClientWebSocket WebsocketClient { get; set; }
        public FortniteApiClient Client { get; set; }

        public Party CurrentParty { get; set; }
        public Presence Presence { get; set; }
        public Platform Platform { get; set; }

        public event Func<MessageReceivedEventArgs, Task> MessageReceived;
        public event Func<Task> Connected;
        public event Func<Task> Disconnected;
        public event Func<NotificationReceivedEventArgs, Task> NotificationReceived;
        public event Func<PingEvent, Task> Ping;
        public event Func<PartyInvitation, Task> PartyInvitation;
        public event Func<GroupChatMessageReceivedEventArgs, Task> PartyChatMessageReceived;
        public event Func<ChatMessageReceivedEventArgs, Task> ChatMessageReceived;

        internal XmppClient(FortniteApiClient client, Platform platform)
        {
            Platform = platform;

            NotificationHandler = new XmppNotificationHandler(this);
            Client = client;

            Jid = $"{Client.CurrentLogin.AccountId}@prod.ol.epicgames.com";
            Resource = $"V2:Fortnite:{platform.ToString().ToUpper()}:{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}";

            WebsocketClient = new ClientWebSocket();
            WebsocketClient.Options.AddSubProtocol("xmpp");
            WebsocketClient.Options.Credentials = new NetworkCredential
            {
                UserName = Jid,
                Password = Client.CurrentLogin.AccessToken
            };
            WebsocketClient.Options.KeepAliveInterval = TimeSpan.FromSeconds(60);
        }

        public async Task StartAsync()
        {
            await WebsocketClient.ConnectAsync(new Uri("wss://xmpp-service-prod.ol.epicgames.com//"),
                CancellationToken.None);
            await OnConnectAsync();

            await OpenAsync();
            await HandleMessagesAsync();
        }

        public async Task OpenAsync()
        {
            var builder = new StringBuilder();
            var writer = XmlWriter.Create(builder, WriterSettings);

            writer.WriteStartElement("open", "urn:ietf:params:xml:ns:xmpp-framing");
            {
                writer.WriteAttributeString("to", "prod.ol.epicgames.com");
                writer.WriteAttributeString("version", "1.0");
            }
            await writer.WriteEndElementAsync().ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);

            await SendMessageAsync(builder.ToString()).ConfigureAwait(false);
        }

        public async Task SendAuthentication()
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\u0000{Client.CurrentLogin.AccountId}\u0000{Client.CurrentLogin.AccessToken}"));

            var builder = new StringBuilder();
            var writer = XmlWriter.Create(builder, WriterSettings);

            writer.WriteStartElement("auth", "urn:ietf:params:xml:ns:xmpp-sasl");
            {
                writer.WriteAttributeString("mechanism", "PLAIN");
            }
            await writer.WriteStringAsync(auth).ConfigureAwait(false);
            await writer.WriteEndElementAsync().ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);

            await SendMessageAsync(builder.ToString()).ConfigureAwait(false);
        }

        public async Task HandleMessagesAsync()
        {
            while (WebsocketClient.State == WebSocketState.Open)
            {
                var stream = new MemoryStream();
                WebSocketReceiveResult receiveResult;
                do
                {
                    var buffer = WebSocket.CreateClientBuffer(1024, 16);
                    receiveResult = await WebsocketClient.ReceiveAsync(buffer, CancellationToken.None);
                    stream.Write(buffer.Array ?? Array.Empty<byte>(), buffer.Offset, receiveResult.Count);
                } while (!receiveResult.EndOfMessage);

                switch (receiveResult.MessageType)
                {
                    case WebSocketMessageType.Close:
                        await DisposeAsync();
                        break;
                    case WebSocketMessageType.Text:
                    {
                        var message = Encoding.UTF8.GetString(stream.ToArray());
                        var document = new XmlDocument();
                        try
                        {
                            document.LoadXml(message.Trim());
                        }
                        catch
                        {
                            return;
                        }

                        switch (document.DocumentElement?.Name)
                        {
                            case "stream:features":
                                await SendAuthentication();
                                break;
                            case "success":
                                await SendIqAsync("_xmpp_bind1");
                                break;
                            case "iq":
                                if (document.DocumentElement.GetAttribute("id").Equals("_xmpp_bind1"))
                                {
                                    await SendIqAsync("_xmpp_session1");
                                }

                                if (document.DocumentElement.GetAttribute("id").Equals("_xmpp_session1"))
                                {
                                    await SendPresenceAsync(new Presence
                                    {
                                        Status = "",
                                        SessionId = "",
                                        Properties = new Dictionary<string, object>()
                                    });
                                }

                                break;
                            case "message":
                                await HandleMessageAsync(document);
                                break;
                        }

                        var args = new MessageReceivedEventArgs
                        {
                            Raw = message,
                            Document = document
                        };
                        MessageReceived?.Invoke(args);
                        break;
                    }
                }
            }

            Debugger.Break();
        }

        public async Task HandleMessageAsync(XmlDocument document)
        {
            var element = document?.DocumentElement;
            switch (element?.GetAttribute("type"))
            {
                case "groupchat":
                    await HandlePartyMessageAsync(document);
                    return;
                case "chat":
                    await HandleChatMessageAsync(document);
                    return;
                case "error":
                case "headline":
                    return;
            }

            await NotificationHandler.HandleNotificationAsync(document);
        }
        
        public async Task HandlePartyMessageAsync(XmlDocument document)
        {
            var element = document?.DocumentElement;
            if(element == null)
            {
                return;
            }

            if (element.InnerText == "Welcome! You created new Multi User Chat Room.")
            {
                return;
            }

            var from = element.GetAttribute("from");
            if (CurrentParty == null || from.Split("@")[0].Replace("Party-", "") != CurrentParty.Id)
            {
                return;
            }

            var id = element.GetAttribute("from").Split("/")[1].Split(":")[1];
            var member = CurrentParty.Members.FirstOrDefault(x => x.Id == id);
            if (member == null)
            {
                return;
            }

            await OnPartyMessageReceivedAsync(new GroupChatMessageReceivedEventArgs
            {
                Party = CurrentParty,
                From = member
            });
        }

        public async Task HandleChatMessageAsync(XmlDocument document)
        {
            var element = document?.DocumentElement;
            if (element == null)
            {
                return;
            }

            var from = element.GetAttribute("from");
            var id = element.GetAttribute("from").Split("@")[0];
            var message = element.GetElementsByTagName("body")[0].InnerText;

            await OnMessageReceivedAsync(new ChatMessageReceivedEventArgs
            {
                From = id,
                Message = message
            });
        }

        public async Task SendPresenceAsync(Presence presence)
        {
            Presence = presence;

            var builder = new StringBuilder();
            var writer = XmlWriter.Create(builder, WriterSettings);

            writer.WriteStartElement("presence");
            writer.WriteStartElement("status");
            {
                await writer.WriteStringAsync(JsonConvert.SerializeObject(presence)).ConfigureAwait(false);
            }
            await writer.WriteEndElementAsync().ConfigureAwait(false);
            writer.WriteStartElement("delay", "urn:xmpp:delay");
            {
                writer.WriteAttributeString("stamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            }
            await writer.WriteEndElementAsync().ConfigureAwait(false);
            await writer.WriteEndElementAsync().ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);

            await SendMessageAsync(builder.ToString()).ConfigureAwait(false);
        }

        public async Task SendIqAsync(string id)
        {
            var builder = new StringBuilder();
            var writer = XmlWriter.Create(builder, WriterSettings);

            switch (id)
            {
                case "_xmpp_bind1":
                {
                    writer.WriteStartElement("iq");
                    {
                        writer.WriteAttributeString("id", id);
                        writer.WriteAttributeString("type", "set");
                        writer.WriteStartElement("bind", "urn:ietf:params:xml:ns:xmpp-bind");
                        {
                            writer.WriteStartElement("resource");
                            {
                                await writer.WriteStringAsync(Resource).ConfigureAwait(false);
                            }
                            await writer.WriteEndElementAsync().ConfigureAwait(false);
                        }
                        await writer.WriteEndElementAsync().ConfigureAwait(false);
                    }
                    await writer.WriteEndElementAsync().ConfigureAwait(false);
                    await writer.FlushAsync().ConfigureAwait(false);

                    await SendMessageAsync(builder.ToString()).ConfigureAwait(false);
                    break;
                }
                case "_xmpp_session1":
                {
                    writer.WriteStartElement("iq");
                    {
                        writer.WriteAttributeString("id", id);
                        writer.WriteAttributeString("type", "set");
                        writer.WriteStartElement("session", "urn:ietf:params:xml:ns:xmpp-session");
                        await writer.WriteEndElementAsync().ConfigureAwait(false);
                    }
                    await writer.WriteEndElementAsync().ConfigureAwait(false);
                    await writer.FlushAsync().ConfigureAwait(false);

                    await SendMessageAsync(builder.ToString()).ConfigureAwait(false);
                    break;
                }
            }
        }

        public async Task JoinPartyChatAsync()
        {
            var builder = new StringBuilder();
            var writer = XmlWriter.Create(builder, WriterSettings);

            writer.WriteStartElement("presence");
            writer.WriteAttributeString("to", $"Party-{CurrentParty.Id}@muc.prod.ol.epicgames.com/{Client.CurrentLogin.DisplayName}:{Client.CurrentLogin.AccountId}:{Resource}");
            writer.WriteStartElement("x", "http://jabber.org/protocol/muc");
            {
                writer.WriteStartElement("history");
                {
                    writer.WriteAttributeString("maxstanzas", "50");
                }
            }
            await writer.WriteEndElementAsync().ConfigureAwait(false);
            await writer.WriteEndElementAsync().ConfigureAwait(false);
            await writer.WriteEndElementAsync().ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);

            await SendMessageAsync(builder.ToString()).ConfigureAwait(false);
        }

        public async Task SendMessageAsync(string message)
        {
            var data = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await WebsocketClient.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None)
                .ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await WebsocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "unavailable", CancellationToken.None);
            WebsocketClient.Dispose();
            await OnDisconnectAsync();
        }

        public async Task OnConnectAsync()
        {
            if (Connected != null)
            {
                await Connected.Invoke();
            }
        }

        public async Task OnDisconnectAsync()
        {
            if (Disconnected != null)
            {
                await Disconnected.Invoke();
            }
        }

        public async Task OnNotificationAsync(NotificationReceivedEventArgs e)
        {
            if (NotificationReceived != null)
            {
                await NotificationReceived.Invoke(e);
            }
        }

        public async Task OnPingAsync(PingEvent e)
        {
            if(Ping != null)
            {
                await Ping.Invoke(e);
            }
        }

        public async Task OnPartyInvitationAsync(PartyInvitation e)
        {
            if(PartyInvitation != null)
            {
                await PartyInvitation.Invoke(e);
            }
        }

        public async Task OnPartyMessageReceivedAsync(GroupChatMessageReceivedEventArgs e)
        {
            if (PartyChatMessageReceived != null)
            {
                await PartyChatMessageReceived.Invoke(e);
            }
        }

        public async Task OnMessageReceivedAsync(ChatMessageReceivedEventArgs e)
        {
            if (ChatMessageReceived != null)
            {
                await ChatMessageReceived.Invoke(e);
            }
        }

    }
}
