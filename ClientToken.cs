using System;
using System.Text;

namespace Fortnite.Net
{
    public static class ClientToken
    {

        public static readonly string FortnitePcGameClient = CreateToken("ec684b8c687f479fadea3cb2ad83f5c6", "e1f31c211f28413186262d37a13fc84d");
        public static readonly string FortniteIosGameClient = CreateToken("3446cd72694c4a4485d81b77adbb2141", "9209d4a5e25a457fb9b07489d313b41a");

        internal static string CreateToken(string clientId, string secret) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));

    }
}