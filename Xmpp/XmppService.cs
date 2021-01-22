namespace Fortnite.Net.Xmpp
{
    public class XmppService
    {

        private readonly FortniteApi _api;

        public XmppService(FortniteApi api)
        {
            _api = api;
            /*var client = new JabberClient
            {
                Server = "prod.ol.epicgames.com",
                NetworkHost = "xmpp-service-prod.ol.epicgames.com",
                Port = 5222,
                Resource = $"V2:Fortnite:WIN::{Guid.NewGuid().ToString().Replace("-", "")}",
                User = _api.LoginModel.AccountId,
                Password = _api.LoginModel.AccessToken,
                AutoRoster = true,
                KeepAlive = 60
            };*/
        }

    }
}