using System;
using System.Text;

namespace Fortnite.Net
{
    public class ClientToken
    {

        public static ClientToken FortnitePcGameClient => CreateToken("ec684b8c687f479fadea3cb2ad83f5c6", "e1f31c211f28413186262d37a13fc84d");
        public static ClientToken FortniteIosGameClient => CreateToken("3446cd72694c4a4485d81b77adbb2141", "9209d4a5e25a457fb9b07489d313b41a");
        public static ClientToken FortniteAndroidGameClient => CreateToken("3f69e56c7649492c8cc29f1af08a8a12", "b51ee9cb12234f50a69efa67ef53812e");
        public static ClientToken FortniteCnGameClient => CreateToken("efe3cbb938804c74b20e109d0efc1548", "6e31bdbae6a44f258474733db74f39ba");
        public static ClientToken FortniteSwitchGameClient => CreateToken("5229dcd3ac3845208b496649092f251b", "e3bd2d3e-bf8c-4857-9e7d-f3d947d220c7");
        public static ClientToken FortniteValkyrieGameClient => CreateToken("3e13c5c57f594a578abe516eecb673fe", "530e316c337e409893c55ec44f22cd62");
        public static ClientToken KairosPc => CreateToken("5b685653b9904c1d92495ee8859dcb00", "7Q2mcmneyuvPmoRYfwM7gfErA6iUjhXr");
        public static ClientToken KairosIos => CreateToken("61d2f70175e84a6bba80a5089e597e1c", "FbiZv3wbiKpvVKrAeMxiR6WhxZWVbrvA");
        public static ClientToken ProdFn => CreateToken("xyza7891343Fr4ZSPkQZ3kaL3I2sX8B5", "F8BVRyHIqmct8cN9KSPbXsJszpiIZEYEFDiySxc1wuA");
        public static ClientToken LauncherAppClient2 => CreateToken("34a02cf8f4414e29b15921876da36f9a", "daafbccc737745039dffe53d94fc76cf");

        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string Base64 { get; set; }

        public static ClientToken CreateToken(string clientId, string secret)
        {
            return new ClientToken
            {
                ClientId = clientId,
                Secret = secret,
                Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"))
            };
        }

    }
}
