using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fortnite.Net.Xmpp.Meta
{
    public class AthenaCosmeticLoadout
    {
        [JsonProperty("AthenaCosmeticLoadout")]
        public AthenaCosmeticLoadoutData Data { get; set; }

        public AthenaCosmeticLoadout(string cid = null, string bid = null, string pid = null)
        {
            Data = new AthenaCosmeticLoadoutData(cid, bid, pid);
        }

    }

    public class AthenaCosmeticLoadoutData
    {
        [JsonProperty("characterDef")]
        public string CharacterItemDefinition { get; set; }

        [JsonProperty("characterEKey")]
        public string CharacterEKey => "";

        [JsonProperty("backpackDef")]
        public string BackpackItemDefinition { get; set; }

        [JsonProperty("backpackEKey")]
        public string BackpackEKey => "";
        
        [JsonProperty("pickaxeDef")]
        public string PickaxeItemDefinition { get; set; }

        [JsonProperty("pickaxeEKey")]
        public string PickaxeEKey => "";

        [JsonProperty("contrailDef")]
        public string ContrailItemDefinition => "None";

        [JsonProperty("contrailEKey")]
        public string ContrailEKey => "";

        [JsonProperty("scratchpad")]
        public object[] Scratchpad => Array.Empty<object>();

        public AthenaCosmeticLoadoutData(string cid = null, string bid = null, string pid = null)
        {
            CharacterItemDefinition = cid == null ?
                "'AthenaCharacterItemDefinition'/Game/Athena/Items/Cosmetics/Characters/CID_563_Athena_Commando_M_RebirthDefaultD.CID_563_Athena_Commando_M_RebirthDefaultD'" :
                $"AthenaCharacterItemDefinition'/Game/Athena/Items/Cosmetics/Characters/{cid}.{cid}";
            BackpackItemDefinition = bid == null ? "None" : $"/Game/Athena/Items/Cosmetics/Backpacks/{bid}.{bid}";
            PickaxeItemDefinition = pid == null ?
                "AthenaPickaxeItemDefinition'/Game/Athena/Items/Cosmetics/Pickaxes/DefaultPickaxe.DefaultPickaxe'" :
                $"AthenaPickaxeItemDefinition'/Game/Athena/Items/Cosmetics/Pickaxes/{pid}.{pid}'";
        }
    }
}
