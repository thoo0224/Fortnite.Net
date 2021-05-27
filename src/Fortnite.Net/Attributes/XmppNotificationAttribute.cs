using System;

namespace Fortnite.Net.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XmppNotificationAttribute : Attribute
    {

        /// <summary>
        /// Type of the notification
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Type of the data
        /// </summary>
        public Type BodyType { get; set; }

        public XmppNotificationAttribute(string type, Type bodyType)
        {
            Type = type;
            BodyType = bodyType;
        }

    }
}
