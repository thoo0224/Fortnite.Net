using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class AssistedChallenge
    {

        [JsonProperty("AssistedChallengeInfo")]
        public AssistedChallengeData Data { get; set; }

        public AssistedChallenge()
        {
            Data = new AssistedChallengeData();
        }

    }
}
