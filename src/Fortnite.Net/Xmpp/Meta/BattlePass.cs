using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class BattlePass
    {

        [JsonProperty("BattlePassInfo")]
        public BattlePassData Data { get; set; }

        public BattlePass(bool isPurchased, int level, int selfXpBoost, int friendXpBoost)
        {
            Data = new BattlePassData
            {
                HasPurchasedPass = isPurchased,
                Level = level,
                SelfXpBoost = selfXpBoost,
                FriendXpBoost = friendXpBoost
            };
        }

    }
}
