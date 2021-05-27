using System;
using System.Collections.Generic;
using System.Text;

namespace Fortnite.Net.Utils
{
    internal static class Preconditions
    {

        public static void NotNullOrEmpty(string str, string name)
        {
            if(string.IsNullOrEmpty(str))
            {
                throw new ArgumentException($"{name} can not be null.", name);
            }
        }

    }
}
