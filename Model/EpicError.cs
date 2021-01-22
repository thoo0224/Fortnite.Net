namespace Fortnite.Net.Model
{
    public class EpicError
    {
        
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string[] MessageVars { get; set; }
        public int NumericErrorCode { get; set; }
        public string OriginatingService { get; set; }
        public string Intent { get; set; }

    }
}