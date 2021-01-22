using System;
using Newtonsoft.Json;

namespace Fortnite.Net.Model.Account
{
    public class VerifyResponse
    {
        
        [JsonProperty("token")] public string Token { get; set; }
        [JsonProperty("session_id")] public string SessionId { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("client_id")] public string ClientId { get; set; }
        [JsonProperty("internal_client")] public bool InternalClient { get; set; }
        [JsonProperty("client_service")] public string ClientService { get; set; }
        [JsonProperty("account_id")] public string AccountId { get; set; }
        [JsonProperty("expires_in")] public long ExpiresIn { get; set; }
        [JsonProperty("expires_at")] public DateTime ExpiresAt { get; set; }
        [JsonProperty("auth_method")] public string AuthMethod { get; set; }
        [JsonProperty("display_name")] public string DisplayName { get; set; }
        [JsonProperty("app")] public string App { get; set; }
        [JsonProperty("in_app_id")] public string InAppId { get; set; }
        [JsonProperty("device_Id")] public string DeviceId { get; set; }
        
    }
}