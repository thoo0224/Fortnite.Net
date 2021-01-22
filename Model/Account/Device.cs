using System;

namespace Fortnite.Net.Model.Account
{
    
    public class Device
    {
        
        public string DeviceId { get; set; }
        public string AccountId { get; set; }
        public string Secret { get; set; }

        public Device(string deviceId, string accountId, string secret)
        {
            DeviceId = deviceId;
            AccountId = accountId;
            Secret = secret;
        }

    }

}