using System.Threading.Tasks;
using Fortnite.Net.Model.Fortnite;

namespace Fortnite.Net.Services
{
    public sealed class FortnitePublicService : EpicService
    {

        public FortnitePublicService(FortniteApi api) : base(api, "https://fortnite-public-service-prod11.ol.epicgames.com/")
       { }

       public async Task<FortCatalogResponse> GetStoreFrontCatalogAsync() =>
            await SendBaseAsync<FortCatalogResponse>("/fortnite/api/storefront/v2/catalog");

    }
}