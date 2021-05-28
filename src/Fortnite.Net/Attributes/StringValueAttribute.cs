using System;

namespace Fortnite.Net.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class StringValueAttribute : Attribute
    {

        public string Value { get; set; }

        public StringValueAttribute(string value)
        {
            Value = value;
        }

    }
}
