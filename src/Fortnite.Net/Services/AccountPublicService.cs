using Fortnite.Net.Enums;
using Fortnite.Net.Objects;
using Fortnite.Net.Objects.Auth;

using RestSharp;

using System.Threading;
using System.Threading.Tasks;
using Fortnite.Net.Utils;

namespace Fortnite.Net.Services
{
    public class AccountPublicService : BaseService
    {

        public override string BaseUrl { get; } = "https://account-public-service-prod.ol.epicgames.com";

        internal AccountPublicService(FortniteApiClient client)
            : base(client) { }

        /// <summary>
        /// Authenticates with an authorization code.
        /// </summary>
        /// <param name="code">Authorization code</param>
        /// <param name="clientToken">The client token, this needs to be the same client for the code.</param>
        /// <param name="cancellationToken">Cancellation clientToken</param>
        /// <returns>Authentication response</returns>
        public async Task<FortniteResponse<AuthResponse>> AuthWithAuthorizationCodeAsync(
            string code = null,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(code, nameof(code));

            var request = new RestRequest("/account/api/oauth/token", Method.POST);
            request.AddHeader("Authorization", $"basic {clientToken?.Base64 ?? Client.DefaultClientToken.Base64}");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code ?? Client.AuthConfig.AuthorizationCode);

            var response = await ExecuteAsync<AuthResponse>(request, token: cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Gets an exchange code
        /// </summary>
        /// <param name="authResponse">Authentication response</param>
        /// <param name="cancellationToken">Cancellation clientToken</param>
        /// <returns>Exchange code response</returns>
        public async Task<FortniteResponse<ExchangeCode>> GetExchangeAsync(
            AuthResponse authResponse,
            CancellationToken cancellationToken = default)
            => await GetExchangeAsync(authResponse.AccessToken, cancellationToken).ConfigureAwait(false);

        /// <inheritdoc cref="GetExchangeAsync(Fortnite.Net.Objects.Auth.AuthResponse,System.Threading.CancellationToken)"/>
        public async Task<FortniteResponse<ExchangeCode>> GetExchangeAsync(
            string accessToken,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(accessToken, nameof(accessToken));

            var request = new RestRequest("/account/api/oauth/exchange", Method.GET);
            request.AddHeader("Authorization", $"bearer {accessToken}");

            var response = await ExecuteAsync<ExchangeCode>(request, token: cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Authenticates with an exchange code
        /// </summary>
        /// <param name="exchangeCode">Exchange code</param>
        /// <param name="clientToken">WebsocketClient clientToken</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Authentication response</returns>
        public async Task<FortniteResponse<AuthResponse>> AuthWithExchangeAsync(
            ExchangeCode exchangeCode,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
            => await AuthWithExchangeAsync(exchangeCode.Code, clientToken, cancellationToken).ConfigureAwait(false);

        /// <inheritdoc cref="AuthWithExchangeAsync(Fortnite.Net.Objects.Auth.ExchangeCode,Fortnite.Net.ClientToken,System.Threading.CancellationToken)"/>
        public async Task<FortniteResponse<AuthResponse>> AuthWithExchangeAsync(
            string exchangeCode,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(exchangeCode, nameof(exchangeCode));

            var request = new RestRequest("/account/api/oauth/token", Method.POST);
            request.AddHeader("Authorization", $"basic {(clientToken ?? Client.DefaultClientToken).Base64}");
            request.AddParameter("grant_type", "exchange_code");
            request.AddParameter("exchange_code", exchangeCode);

            var response = await ExecuteAsync<AuthResponse>(request, token: cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Creates a device.
        /// </summary>
        /// <param name="authResponse">Authentication response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The device that has been created.</returns>
        public async Task<FortniteResponse<Device>> CreateDeviceAsync(
            AuthResponse authResponse,
            CancellationToken cancellationToken = default)
        {
            return await CreateDeviceAsync(authResponse.AccessToken, authResponse.AccountId, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="CreateDeviceAsync(Fortnite.Net.Objects.Auth.AuthResponse,System.Threading.CancellationToken)" />
        /// <param name="accessToken">Access token</param>
        /// <param name="accountId">Id of the account</param>
        /// <param name="cancellationToken"></param>
        public async Task<FortniteResponse<Device>> CreateDeviceAsync(
            string accessToken,
            string accountId,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(accessToken, nameof(accessToken));
            Preconditions.NotNullOrEmpty(accountId, nameof(accountId));

            var request = new RestRequest($"/account/api/public/account/{accountId}/deviceAuth", Method.POST);
            request.AddHeader("Authorization", $"bearer {accessToken}");

            var response = await ExecuteAsync<Device>(request, token: cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Authenticates with the device provided.
        /// </summary>
        /// <param name="device">Device</param>
        /// <param name="clientToken">WebsocketClient token</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Authentication response</returns>
        public async Task<FortniteResponse<AuthResponse>> AuthWithDeviceAsync(
            Device device,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
        {
            return await AuthWithDeviceAsync(device.AccountId, device.DeviceId, device.Secret, clientToken,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="AuthWithDeviceAsync(Fortnite.Net.Objects.Auth.Device,Fortnite.Net.ClientToken,System.Threading.CancellationToken)"/>
        /// <param name="accountId"></param>
        /// <param name="deviceId"></param>
        /// <param name="secret"></param>
        /// <param name="clientToken">WebsocketClient token</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task<FortniteResponse<AuthResponse>> AuthWithDeviceAsync(
            string accountId,
            string deviceId,
            string secret,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(accountId, nameof(accountId));
            Preconditions.NotNullOrEmpty(deviceId, nameof(deviceId));
            Preconditions.NotNullOrEmpty(secret, nameof(secret));

            var request = new RestRequest("/account/api/oauth/token", Method.POST);
            request.AddHeader("Authorization", $"basic {clientToken?.Base64 ?? Client.DefaultClientToken.Base64}");
            request.AddParameter("grant_type", "device_auth");
            request.AddParameter("account_id", accountId);
            request.AddParameter("device_id", deviceId);
            request.AddParameter("secret", secret);

            var response = await ExecuteAsync<AuthResponse>(request, token: cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Kills sessions
        /// </summary>
        /// <param name="killType">The kill type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task KillSessionsAsync(
            SessionKillType killType,
            CancellationToken cancellationToken = default)
        {
            var request = new RestRequest($"/account/api/oauth/sessions/kill?killType={killType}", Method.DELETE);
            await ExecuteAsync(request, true, cancellationToken);
        }

        /// <summary>
        /// Kills a session
        /// </summary>
        /// <param name="accessToken">The AccessToken from the session to kill</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task KillSessionAsync(
            string accessToken,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(accessToken, nameof(accessToken));

            var request = new RestRequest($"/account/api/oauth/sessions/kill/{accessToken}");
            await ExecuteAsync(request, true, cancellationToken);
        }

        public async Task KillCurrentSessionAsync(CancellationToken cancellationToken = default)
        {
            await KillSessionAsync(Client.CurrentLogin.AccessToken, cancellationToken)
                .ConfigureAwait(false);
        }

        /*
         * TODO:
         *
         * FindAccounts
         * FindAccount
         * EditAccount
         * GetAccountMetaData
         * GetDeviceAuths
         * GetDeviceAuth
         * DeleteDeviceAuth
         * GetExternalAuths
         * GetExternalAuth
         * CreateExternalAuth
         * RemoveExternalAuth
         * FindAccountByDisplayName
         * FindAccountByEmail
         * FindAccountById
         * QuerySsoDomains
         */

    }
}
