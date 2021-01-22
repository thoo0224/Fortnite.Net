using Newtonsoft.Json;

namespace Fortnite.Net.Model.Account
{

    public class LoginModel
    {
        
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("expires_in")] public long ExpiresIn { get; set; }
        [JsonProperty("expires_at")] public string ExpiresAt { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
        [JsonProperty("refresh_expires")] public long RefreshExpires { get; set; }
        [JsonProperty("refresh_expires_at")] public string RefreshExpiresAt { get; set; }
        [JsonProperty("account_id")] public string AccountId { get; set; }
        [JsonProperty("client_id")] public string ClientId { get; set; }
        [JsonProperty("internal_client")] public string InternalClient { get; set; }
        [JsonProperty("client_service")] public string ClientService { get; set; }
        [JsonProperty("displayName")] public string DisplayName { get; set; }
        [JsonProperty("app")] public string App { get; set; }
        [JsonProperty("in_app_id")] public string InAppId { get; set; }
        [JsonProperty("device_id")] public string DeviceId { get; set; }
        
    }
}