﻿using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class FortGameplayStats
    {

        [JsonProperty("state")]
        public string State { get; set; } = "";

        [JsonProperty("playlist")]
        public string Playlist { get; set; } = "None";

        [JsonProperty("numKills")]
        public int Kills { get; set; }

        [JsonProperty("bFellToDeath")]
        public bool FellToDeath => false;

    }
}
