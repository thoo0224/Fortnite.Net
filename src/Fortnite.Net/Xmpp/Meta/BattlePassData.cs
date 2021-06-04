using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class BattlePassData
    {

        [JsonProperty("bHasPurchasedPass")]
        public bool HasPurchasedPass { get; set; }

        [JsonProperty("PassLevel")]
        public int Level { get; set; }

        [JsonProperty("selfBoostXp")]
        public int SelfXpBoost { get; set; }

        [JsonProperty("friendBoostXp")]
        public int FriendXpBoost { get; set; }

    }
}
