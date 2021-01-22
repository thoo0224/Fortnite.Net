using System;
using System.Threading.Tasks;
using Fortnite.Net.Model.Friend;
using Fortnite.Net.Utils;
using RestSharp;

namespace Fortnite.Net.Services
{
    public sealed class FriendsPublicService : EpicService
    { 
        
        public FriendsPublicService(FortniteApi api) : base(api, "https://friends-public-service-prod.ol.epicgames.com/")
        { }

        public async Task<Friend[]> GetFriendsAsync() =>
            await SendBaseAsync<Friend[]>($"/friends/api/public/friends/{_api.LoginModel.AccountId}");

        public Friend[] GetFriends() =>
            GetFriendsAsync().GetAwaiter().GetResult();

        public async Task<object> InviteOrAcceptAsync(string friend) =>
            await SendBaseAsync<object>($"/friends/api/public/friends/{_api.LoginModel.AccountId}/{friend}", Method.POST);

        public object InviteOrAccept(string friend) =>
            InviteOrAcceptAsync(friend).GetAwaiter().GetResult();
        
        public async Task<object> RemoveOrRejectAsync(string friend) =>
            await SendBaseAsync<object>($"/friends/api/public/friends/{_api.LoginModel.AccountId}/{friend}", Method.DELETE);

        public object RemoveOrReject(string friend) =>
            InviteOrAcceptAsync(friend).GetAwaiter().GetResult();

        public async Task<BlockedUserList> GetBlockedListAsync() =>
            await SendBaseAsync<BlockedUserList>($"/friends/api/public/blocklist/{_api.LoginModel.AccountId}");

        public BlockedUserList GetBlockedList() =>
            GetBlockedListAsync().GetAwaiter().GetResult();

    }
}