using Fortnite.Net.Attributes;

namespace Fortnite.Net.Enums
{
    public enum GrantType
    {

        [StringValue("authorization_code")]
        AuthorizationCode,

        [StringValue("client_credentials")]
        ClientCredentials,

        [StringValue("device_code")]
        DeviceCode,

        [StringValue("device_auth")]
        DeviceAuth,

        [StringValue("exchange_code")]
        ExchangeCode,

        [StringValue("external_auth")]
        ExternalAuth,

        [StringValue("opt")]
        Opt,

        [StringValue("password")]
        Password,

        [StringValue("refresh_token")]
        RefreshToken,

        [StringValue("token_to_token")]
        TokenToToken

    }
}
