using Fortnite.Net.Xmpp.Meta;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Fortnite.Net.Objects.Party
{
    public class PartyMember
    {

        public string Id { get; set; }

        public Dictionary<string, string> Meta { get; set; }

        public List<PartyMemberConnection> Connections { get; set; }

        public int Revision { get; set; }

        [JsonProperty("updated_at")] 
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("joined_at")] 
        public DateTime JoinedAt { get; set; }

        public string Role { get; set; }

        internal static Dictionary<string, object> SchemaMeta
            => new Dictionary<string, object>
            {
                { "Default:Location_s", "PreLobby" },
                { "Default:FrontendEmote_j", JsonConvert.SerializeObject(new FrontendEmote(null), NewtonsoftSerializer.SerializerSettings) },
                { "Default:NumAthenaPlayersLeft_U", "0" },
                { "Default:SpectateAPartyMemberAvailable_b", "false" },
                { "Default:Utc:timeStartedMatchAthena_s", "0001-01-01T00:00:00.000Z" },
                { "Default:LobbyState_j", JsonConvert.SerializeObject(new LobbyState(), NewtonsoftSerializer.SerializerSettings) },
                { "Default:AssistedChallengeInfo_j", JsonConvert.SerializeObject(new AssistedChallenge(), NewtonsoftSerializer.SerializerSettings) },
                { "Default:FeatDefinition_s", "None" },
                { "Default:MemberSquadAssignmentRequest_j", JsonConvert.SerializeObject(new SquadAssignmentRequest(), NewtonsoftSerializer.SerializerSettings) },
                { "Default:VoiceChatStatus_s", "Disabled" },
                { "Default:SidekickStatus_s", "None" },
                { "Default:AthenaCosmeticLoadout_j", JsonConvert.SerializeObject(new AthenaCosmeticLoadout("CID_556_Athena_Commando_F_RebirthDefaultA"), NewtonsoftSerializer.SerializerSettings) },
                { "Default:AthenaCosmeticLoadoutVariants_j", JsonConvert.SerializeObject(new AthenaCosmeticLoadoutVariants("athenaCharacter", "Material", "Mat2"), NewtonsoftSerializer.SerializerSettings)},
                { "Default:AthenaBannerInfo_j", JsonConvert.SerializeObject(new AthenaBannerInfo("brseason01", "defaultcolor19", 69), NewtonsoftSerializer.SerializerSettings)},
                { "Default:BattlePassInfo_j", JsonConvert.SerializeObject(new BattlePass(true, 100, 0, 0), NewtonsoftSerializer.SerializerSettings) },
                { "Default:PlatformData_j", JsonConvert.SerializeObject(new PlatformMeta(), NewtonsoftSerializer.SerializerSettings) },
                { "Default:CrossplayPreference_s", "OptedIn" }
            };

    }
}
