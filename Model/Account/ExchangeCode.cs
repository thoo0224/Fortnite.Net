namespace Fortnite.Net.Model.Account
{
    public class ExchangeCode
    {
        
        public int ExpiresInSeconds { get; set; }
        public string Code { get; set; }
        public string CreatingClientId { get; set; }
        
    }
}