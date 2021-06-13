namespace Fortnite.Net.Services
{
    public class FortniteService : BaseService
    {

        public override string BaseUrl => "https://fortnite-public-service-prod11.ol.epicgames.com/fortnite/";

        internal FortniteService(FortniteApiClient client) : base(client)
        {
        }

    }
}
