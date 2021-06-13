using System;
using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Fortnite.Net.Objects.Auth
{
    public class BaseOAuth
    {

        [J("account_id")] public string AccountId { get; set; }
        [J("client_id")] public string ClientId { get; set; }
        [J("client_service")] public string ClientService { get; set; }

        // Non existent if X-Epic-Device-Id header was not provided
        [J("device_id")] public string DeviceId { get; set; }
        [J("expires_at")] public DateTime ExpiresAt { get; set; }
        [J("expires_in")] public long ExpiresIn { get; set; }
        [J("in_app_id")] public string InAppId { get; set; }
        [J("internal_client")] public bool InternalClient { get; set; }
        [J("lastPasswordValidation")] public DateTime LastPasswordValidation { get; set; }
        [J("perms")] public List<Permission> Perms { get; set; }
        [J("token_type")] public string TokenType { get; set; }

        public string DisplayName { get; set; }

    }
}
