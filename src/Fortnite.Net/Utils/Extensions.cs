using System;
using System.Reflection;
using Fortnite.Net.Attributes;

namespace Fortnite.Net.Utils
{
    public static class Extensions
    {

        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attribute = fieldInfo.GetCustomAttribute<StringValueAttribute>();

            return attribute.Value;
        }

    }
}
