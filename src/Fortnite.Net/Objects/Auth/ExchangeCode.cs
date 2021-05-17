namespace Fortnite.Net.Objects.Auth
{
    public class ExchangeCode
    {

        public int ExpiresInSeconds { get; set; }
        public string Code { get; set; }
        public string CreatingClientId { get; set; }

    }
}
