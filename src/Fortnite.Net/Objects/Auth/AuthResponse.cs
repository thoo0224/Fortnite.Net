using System;

using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Fortnite.Net.Objects.Auth
{
    public class AuthResponse : BaseOAuth
    {

        [J("access_token")] public string AccessToken { get; set; }
        public string App { get; set; }
        [J("refresh_expires")] public string RefreshExpires { get; set; }
        [J("refresh_expires_at")] public DateTime RefreshExpiresAt { get; set; }
        [J("refresh_token")] public string RefreshToken { get; set; }

    }
}
