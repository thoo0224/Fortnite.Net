using Fortnite.Net.Attributes;
using Fortnite.Net.Xmpp.EventArgs;
using Fortnite.Net.Xmpp.Events;
using Fortnite.Net.Xmpp.Notifications;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace Fortnite.Net.Xmpp
{
    public class XmppNotificationHandler
    {

        private readonly XmppClient _client;

        private readonly Dictionary<string, INotifications> _registeredNotifications;
        private readonly Dictionary<string, (MethodInfo method, XmppNotificationAttribute attribute)> _notificationHandlers;

        public XmppNotificationHandler(XmppClient client)
        {
            _registeredNotifications = new Dictionary<string, INotifications>();
            _notificationHandlers = new Dictionary<string, (MethodInfo method, XmppNotificationAttribute attribute)>();
            _client = client;

            _registeredNotifications["com.epicgames.social.party.notification.v0"] = new PartyNotifications(_client);

            foreach (var (notificationTypeBase, notifications) in _registeredNotifications)
            {
                var type = notifications.GetType();
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    var attribute = method.GetCustomAttribute<XmppNotificationAttribute>();
                    if (attribute == null)
                    {
                        continue;
                    }

                    var fullType = $"{notificationTypeBase}.{attribute.Type}";
                    _notificationHandlers[fullType] = (method, attribute);
                }
            }
        }

        public async Task HandleNotificationAsync(XmlDocument document)
        {
            var content = document?.DocumentElement?.InnerText!;
            var baseBody = JsonConvert.DeserializeObject<BaseEvent>(content ?? throw new InvalidOperationException(), NewtonsoftSerializer.SerializerSettings);
            if(baseBody == null)
            {
                return;
            }

            await _client.OnNotificationAsync(new NotificationReceivedEventArgs 
            {
                Type = baseBody.Type,
                Body = content
            });
            foreach (var (_, (method, attribute)) in _notificationHandlers)
            {
                if (attribute.Type.Equals(baseBody.Type))
                {
                    var parameter = JsonConvert.DeserializeObject(content, attribute.BodyType);
                    var type = baseBody.Type;
                    var pos = type.LastIndexOf('.');
                    var notificationType = type.Substring(0, pos);
                    var notificationClass = _registeredNotifications[notificationType];

                    method.Invoke(notificationClass, new object[] { parameter });
                }
            }
        }

    }
}
