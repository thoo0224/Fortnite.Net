using System;

namespace Fortnite.Net.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XmppNotificationAttribute : Attribute
    {

        public string Type { get; set; }
        public Type BodyType { get; set; }

        public XmppNotificationAttribute(string type, Type bodyType)
        {
            Type = type;
            BodyType = bodyType;
        }

    }
}
