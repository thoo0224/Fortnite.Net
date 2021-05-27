using System;
using System.Runtime.CompilerServices;

namespace Fortnite.Net.Utils
{
    internal static class Preconditions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNullOrEmpty(string str, string name)
        {
            if(string.IsNullOrEmpty(str))
            {
                throw new ArgumentException($"{name} can not be null.", name);
            }
        }

    }
}
