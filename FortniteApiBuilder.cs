#nullable enable
using System.IO;
using Fortnite.Net.Model.Account;
using Newtonsoft.Json;

namespace Fortnite.Net
{
    public class FortniteApiBuilder
    {

        public string ExchangeToken { set; get; } = null!;
        public string AuthorizationCode { get; set; } = null!;
        public Device Device { get; set; } = null!;
        public string? ClientToken { get; set; }

        public FortniteApiBuilder SetExchangeToken(string exchangeToken)
        {
            ExchangeToken = exchangeToken;
            return this;
        }

        public FortniteApiBuilder SetAuthorizationCode(string authorizationCode)
        {
            AuthorizationCode = authorizationCode;
            return this;
        }

        public FortniteApiBuilder SetDevice(string deviceId, string accountId, string secret)
        {
            Device = new Device(deviceId, accountId, secret);
            return this;
        }

        public FortniteApiBuilder SetDeviceFromFile(string file)
        {
            var content = File.ReadAllText(file);
            Device = JsonConvert.DeserializeObject<Device>(content);
            return this;
        }

        public FortniteApiBuilder SetClientToken(string clientToken)
        {
            ClientToken = clientToken;
            return this;
        }

        public FortniteApi Build() =>
            new FortniteApi(ExchangeToken, AuthorizationCode, Device, ClientToken ?? Net.ClientToken.FortnitePcGameClient);
        
    }
}