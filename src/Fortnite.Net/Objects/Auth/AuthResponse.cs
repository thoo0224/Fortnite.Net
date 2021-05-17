using System;

using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Fortnite.Net.Objects.Auth
{
    public class AuthResponse : BaseOAuth
    {

        [J("access_token")] public string AccessToken { get; set; }
        public string App { get; set; }
        public string RefreshExpires { get; set; }
        public DateTime RefreshExpiresAt { get; set; }
        public string RefreshToken { get; set; }

    }
}
