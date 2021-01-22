using System.Threading.Tasks;
using Fortnite.Net.Model.Account;
using RestSharp;

namespace Fortnite.Net.Services
{
    public sealed class AccountPublicService : EpicService
    {

        public AccountPublicService(FortniteApi api) : base(api, "https://account-public-service-prod.ol.epicgames.com/")
        { }

        public async Task<string[]> GetSsoDomainsAsync() =>
            await SendBaseAsync<string[]>("/account/api/epicdomains/ssodomains");

        public string[] GetSsoDomains() =>
            GetSsoDomainsAsync().GetAwaiter().GetResult();

        public async Task<GameProfile> GetUserFromEmailAsync(string email) =>
            await SendBaseAsync<GameProfile>($"/account/api/public/account/email/{email}");

        public GameProfile GetUserFromEmail(string email) =>
            GetUserFromEmailAsync(email).GetAwaiter().GetResult();

        public async Task<VerifyResponse> VerifyTokenAsync(bool includePerms = false) =>
            await SendBaseAsync<VerifyResponse>($"/account/api/oauth/verify?includePerms={includePerms}");

        public async Task<VerifyResponse> VerifyTokenAsync(LoginModel loginModel, bool includePerms = false) =>
            await VerifyTokenAsync(loginModel.AccountId, includePerms);

        public async Task<VerifyResponse> VerifyTokenAsync(string token, bool includePerms = false) =>
            await SendBaseAsync<VerifyResponse>($"/account/api/oauth/verify?includePerms={includePerms}", Method.GET, false, req =>
            {
                req.AddHeader("Authorization", $"bearer {token}");
            });

        public VerifyResponse VerifyToken(bool includePerms = false) =>
            VerifyTokenAsync(includePerms).GetAwaiter().GetResult();

        public VerifyResponse VerifyToken(LoginModel loginModel, bool includePerms = false) =>
            VerifyTokenAsync(loginModel.AccountId, includePerms).GetAwaiter().GetResult();

        public VerifyResponse VerifyToken(string token, bool includePerms = false) =>
            VerifyTokenAsync(token, includePerms).GetAwaiter().GetResult();

    }
}