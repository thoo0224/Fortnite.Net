using System;

namespace Fortnite.Net
{
    public class FortniteApiException : Exception
    {
        
        public FortniteApiException(string message) : base(message) {}
        
    }
}