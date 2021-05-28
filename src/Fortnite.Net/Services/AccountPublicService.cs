using Fortnite.Net.Enums;
using Fortnite.Net.Objects;
using Fortnite.Net.Objects.Account;
using Fortnite.Net.Objects.Auth;
using Fortnite.Net.Utils;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fortnite.Net.Exceptions;

namespace Fortnite.Net.Services
{
    public class AccountPublicService : BaseService
    {

        public override string BaseUrl { get; } = "https://account-public-service-prod.ol.epicgames.com";

        internal AccountPublicService(FortniteApiClient client)
            : base(client)
        {
        }

        /// <summary>
        /// Authenticates with <paramref name="grantType"/>
        /// </summary>
        /// <param name="grantType"><see cref="GrantType"/></param>
        /// <param name="token">Client token, if null it will use the one provided in the client builder.</param>
        /// <param name="fields">The fields for the request</param>
        /// <returns>The Fortnite response</returns>
        public async Task<FortniteResponse<AuthResponse>> GetAccessTokenAsync(
            GrantType grantType,
            ClientToken token = null,
            params (string Key, string value)[] fields)
        {
            var response = await GetAccessTokenAsync(grantType, token, default, fields)
                .ConfigureAwait(false);

            return response;
        }

        /// <inheritdoc cref="GetAccessTokenAsync(Fortnite.Net.Enums.GrantType,Fortnite.Net.ClientToken,System.Tuple"/>
        public async Task<FortniteResponse<AuthResponse>> GetAccessTokenAsync(
            GrantType grantType,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default,
            params (string Key, string value)[] fields)
        {
            var request = new RestRequest("/account/api/oauth/token", Method.POST);
            request.AddHeader("Authorization", $"basic {clientToken?.Base64 ?? Client.DefaultClientToken.Base64}");
            request.AddParameter("grant_type", grantType.GetStringValue());

            foreach (var (k, v) in fields)
            {
                request.AddParameter(k, v);
            }

            var response = await ExecuteAsync<AuthResponse>(request, token: cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

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

            var response = await GetAccessTokenAsync(GrantType.AuthorizationCode, clientToken, cancellationToken,
                ("code", code)).ConfigureAwait(false);
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

            var response = await GetAccessTokenAsync(GrantType.ExchangeCode, clientToken, cancellationToken,
                ("exchange_code", exchangeCode)).ConfigureAwait(false);
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

            var response = await GetAccessTokenAsync(GrantType.DeviceAuth, clientToken, cancellationToken,
                ("account_id", accountId),
                ("device_id", deviceId),
                ("secret", secret)).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Gets all the device auths
        /// </summary>
        /// <param name="device">Device</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Fortnite response</returns>
        public async Task<FortniteResponse<Device[]>> GetDeviceAuthsAsync(
            Device device,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNull(device, nameof(device));

            var response = await GetDeviceAuthsAsync(device.AccountId, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <inheritdoc cref="GetDeviceAuthsAsync(Fortnite.Net.Objects.Auth.Device,System.Threading.CancellationToken)"/>
        /// <param name="accountId">Id of the account</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task<FortniteResponse<Device[]>> GetDeviceAuthsAsync(
            string accountId,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(accountId, nameof(accountId));

            var request = new RestRequest($"/account/api/public/account/{accountId}/deviceAuth");
            var response = await ExecuteAsync<Device[]>(request, true, cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        /// <inheritdoc cref="GetDeviceAuthsAsync(Fortnite.Net.Objects.Auth.Device,System.Threading.CancellationToken)"/>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task<FortniteResponse<Device[]>> GetDeviceAuthsAsync(
            CancellationToken cancellationToken = default)
        {
            if (!Client.IsLoggedIn || Client.CurrentLogin == null)
            {
                throw new FortniteException("You need to be logged in to use this.");
            }

            var response = await GetDeviceAuthsAsync(Client.CurrentLogin.AccountId, cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Gets the device auth
        /// </summary>
        /// <param name="device">The device</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The Fortnite response</returns>
        public async Task<FortniteResponse<Device>> GetDeviceAuthAsync(
            Device device,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNull(device, nameof(device));

            var response = await GetDeviceAuthAsync(device.AccountId, device.DeviceId, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <inheritdoc cref="GetDeviceAuthAsync(Fortnite.Net.Objects.Auth.Device,System.Threading.CancellationToken)"/>
        /// <param name="accountId">The account id</param>
        /// <param name="deviceId">The device id</param>
        /// <param name="cancellationToken">The cancellation token</param>
        public async Task<FortniteResponse<Device>> GetDeviceAuthAsync(
            string accountId,
            string deviceId,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNull(accountId, nameof(accountId));
            Preconditions.NotNull(deviceId, nameof(deviceId));

            var request = new RestRequest($"/account/api/public/account/{accountId}/deviceAuth/{deviceId}");
            var response = await ExecuteAsync<Device>(request, true, cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Deletes the device auth
        /// </summary>
        /// <param name="device">The device</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The Fortnite response</returns>
        public async Task<FortniteResponse<Device>> DeleteDeviceAuthAsync(
            Device device,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNull(device, nameof(device));

            var response = await DeleteDeviceAuthAsync(device.AccountId, device.DeviceId, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        /// <inheritdoc cref="DeleteDeviceAuthAsync(Fortnite.Net.Objects.Auth.Device,System.Threading.CancellationToken)"/>
        /// <param name="accountId">The account id</param>
        /// <param name="deviceId">The device id</param>
        /// <param name="cancellationToken">The cancellation token</param>
        public async Task<FortniteResponse<Device>> DeleteDeviceAuthAsync(
            string accountId,
            string deviceId,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNull(accountId, nameof(accountId));
            Preconditions.NotNull(deviceId, nameof(deviceId));

            var request = new RestRequest($"/account/api/public/account/{accountId}/deviceAuth/{deviceId}", Method.DELETE);
            var response = await ExecuteAsync<Device>(request, true, cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Kills sessions
        /// </summary>
        /// <param name="killType">The kill type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Fortnite response</returns>
        public async Task<FortniteResponse> KillSessionsAsync(
            SessionKillType killType,
            CancellationToken cancellationToken = default)
        {
            var request = new RestRequest($"/account/api/oauth/sessions/kill?killType={killType}", Method.DELETE);
            var response = await ExecuteAsync(request, true, cancellationToken)
                .ConfigureAwait(false);

            if (killType == SessionKillType.ALL)
            {
                Client.IsLoggedIn = false;
                Client.CurrentLogin = null;
            }

            return response;
        }

        /// <summary>
        /// Kills a session
        /// </summary>
        /// <param name="accessToken">The AccessToken from the session to kill</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Fortnite response</returns>
        public async Task<FortniteResponse> KillSessionAsync(
            string accessToken,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(accessToken, nameof(accessToken));

            var request = new RestRequest($"/account/api/oauth/sessions/kill/{accessToken}");
            var response = await ExecuteAsync(request, true, cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Kills the current session
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Fortnite response</returns>
        public async Task<FortniteResponse> KillCurrentSessionAsync(CancellationToken cancellationToken = default)
        {
            if (!Client.IsLoggedIn || Client.CurrentLogin == null)
            {
                throw new InvalidOperationException(
                    "Tried killing the current session but the client was not logged in.");
            }

            var response = await KillSessionAsync(Client.CurrentLogin.AccessToken, cancellationToken)
                .ConfigureAwait(false);

            Client.IsLoggedIn = false;
            Client.CurrentLogin = null;

            return response;
        }

        /// <summary>
        /// Finds a profile by display name
        /// </summary>
        /// <param name="displayName">The display name of the account</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Fortnite response</returns>
        public async Task<FortniteResponse<GameProfile>> FindAccountByDisplayNameAsync(
            string displayName,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(displayName, nameof(displayName));

            var request = new RestRequest($"/account/api/public/account/displayName/{displayName}");
            var response = await ExecuteAsync<GameProfile>(request, true, cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Finds a profile by email
        /// </summary>
        /// <param name="accountEmail">The email of the account</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Fortnite response</returns>
        public async Task<FortniteResponse<GameProfile>> FindAccountByEmailAsync(
            string accountEmail,
            CancellationToken cancellationToken = default)
        {
            Preconditions.NotNullOrEmpty(accountEmail, nameof(accountEmail));

            var request = new RestRequest($"/account/api/public/account/email/{accountEmail}");
            var response = await ExecuteAsync<GameProfile>(request, true, cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        public async Task<FortniteResponse<List<string>>> QuerySsoDomainsAsync(CancellationToken cancellationToken = default)
        {
            var request = new RestRequest("/account/api/epicdomains/ssodomains");
            var response = await ExecuteAsync<List<string>>(request, token: cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        /*
         * TODO:
         *
         * EditAccount
         * GetAccountMetaData
         * GetExternalAuth
         * CreateExternalAuth
         * RemoveExternalAuth
         * FindAccountById
         */

    }
}