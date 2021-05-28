namespace Fortnite.Net.Objects.Auth
{
    public class Device
    {
        
        public string DeviceId { get; set; }
        public string AccountId { get; set; }
        public string Secret { get; set; }

        public string UserAgent { get; set; }
        public DeviceLocation Created { get; set; }
        public DeviceLocation LastAccess { get; set; }

        public Device(string deviceId, string accountId, string secret)
        {
            DeviceId = deviceId;
            AccountId = accountId;
            Secret = secret;
        }


    }
}
