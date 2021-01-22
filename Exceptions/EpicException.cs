using System;
using Fortnite.Net.Model;

namespace Fortnite.Net.Exceptions
{
    public class EpicException : Exception
    {
        
        public EpicException(EpicError error) : 
            base($"{error.ErrorMessage} ({error.ErrorCode})") {}

    }
}