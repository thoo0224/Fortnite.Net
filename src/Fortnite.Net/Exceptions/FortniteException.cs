using Fortnite.Net.Objects.Epic;

using System;

namespace Fortnite.Net.Exceptions
{
    public class FortniteException : Exception
    {

        public EpicError EpicError { get; set; }

        public FortniteException(string message, EpicError epicError)
            : base($"{message} {(epicError != null ? $"({epicError.ErrorMessage})" : "")}") { }

        public FortniteException(string message, string epicMessage = null)
            : base($"{message} {(epicMessage != null ? $"({epicMessage})" : "")}") { }

    }
}
