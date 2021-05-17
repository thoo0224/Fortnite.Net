using System.Diagnostics;

namespace Fortnite.Net.Objects.Epic
{
    [DebuggerDisplay("{" + nameof(ErrorMessage) + "}: {" + nameof(ErrorDescription) + "}")]
    public class EpicError
    {

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public object[] MessageVars { get; set; }
        public int NumericErrorCode { get; set; }
        public string OriginatingService { get; set; }
        public string Intent { get; set; }
        public string ErrorDescription { get; set; }
        public string Error { get; set; }

    }
}
