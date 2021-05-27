using Fortnite.Net.Objects.Epic;

using System;

namespace Fortnite.Net.Exceptions
{
    public class FortniteException : Exception
    {

        public EpicError EpicError { get; set; }

        public FortniteException(string message)
            : base(message) { }

        public FortniteException(string message, EpicError epicError)
            : base($"{message} Epic Message: {epicError.ErrorMessage}")
        {
            EpicError = epicError;
        }

    }
}
