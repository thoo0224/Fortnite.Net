using Fortnite.Net.Attributes;

namespace Fortnite.Net.Enums
{
    public enum GrantType
    {

        [StringValueAttribute("authorization_code")]
        AuthorizationCode,

        [StringValueAttribute("client_credentials")]
        ClientCredentials,

        [StringValueAttribute("device_code")]
        DeviceCode,

        [StringValueAttribute("device_auth")]
        DeviceAuth,

        [StringValueAttribute("exchange_code")]
        ExchangeCode,

        [StringValueAttribute("external_auth")]
        ExternalAuth,

        [StringValueAttribute("opt")]
        Opt,

        [StringValueAttribute("password")]
        Password,

        [StringValueAttribute("refresh_token")]
        RefreshToken,

        [StringValueAttribute("token_to_token")]
        TokenToToken

    }
}
