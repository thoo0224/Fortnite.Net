using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class AthenaBannerInfo
    {
        [JsonProperty("AthenaBannerInfo")]
        public AthenaBannerInfoData Data { get; set; }

        public AthenaBannerInfo(string bannerIconId, string bannerColorId, int seasonLevel)
        {
            Data = new AthenaBannerInfoData(bannerIconId, bannerColorId, seasonLevel);
        }

    }

    public class AthenaBannerInfoData
    {
        [JsonProperty("bannerIconId")]
        public string BannerIconId { get; set; }

        [JsonProperty("bannerColorId")]
        public string BannerColorId { get; set; }

        [JsonProperty("seasonLevel")]
        public int SeasonLevel { get; set; }

        public AthenaBannerInfoData(string bannerIconId, string bannerColorId, int seasonLevel)
        {
            BannerIconId = bannerIconId;
            BannerColorId = bannerColorId;
            SeasonLevel = seasonLevel;
        }
    }
}
