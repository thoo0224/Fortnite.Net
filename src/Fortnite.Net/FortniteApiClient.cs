using Fortnite.Net.Config;
using Fortnite.Net.Enums;
using Fortnite.Net.Exceptions;
using Fortnite.Net.Objects.Auth;
using Fortnite.Net.Services;
using Fortnite.Net.Xmpp;

using RestSharp;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fortnite.Net
{
    /// <summary>
    /// The Fortnite Api Client
    /// It's recommended to dispose the client when you are done since it kills the current session.
    /// If this is not disposed, you may have too many access tokens active and you will need to wait a few hours
    /// to use the api again.
    /// </summary>
    public class FortniteApiClient : IAsyncDisposable
    {

        internal AuthConfig AuthConfig { get; set; }
        internal ClientToken DefaultClientToken { get; set; }

        /// <summary>
        /// Fired on Authentication. (This will not fire on refresh)
        /// </summary>
        public event Func<AuthResponse, Task> Login;

        /// <summary>
        /// The current authentication session
        /// </summary>
        public AuthResponse CurrentLogin { get; set; }

        /// <summary>
        /// If the user is logged in
        /// </summary>
        public bool IsLoggedIn { get; set; }

        private readonly Lazy<XmppClient> _xmppClient;
        internal XmppClient XmppClient
        {
            get
            {
                VerifyLogin();
                return _xmppClient.Value;
            }
        }

        private PartyService _partyService;

        /// <summary>
        /// Contains most/all of the party endpoints.
        /// </summary>
        public PartyService PartyService
        {
            get
            {
                VerifyLogin();
                return _partyService;
            }
            set => _partyService = value;
        }

        /// <summary>
        /// Contains most/all of the account endpoints.
        /// </summary>
        public AccountPublicService AccountPublicService { get; set; }

        private readonly Action<RestClient> _restClientAction;

        internal FortniteApiClient(
            AuthConfig authConfig,
            Action<RestClient> restClientActions,
            ClientToken defaultClientToken,
            Platform platform)
        {
            AuthConfig = authConfig;
            DefaultClientToken = defaultClientToken;
            _restClientAction = restClientActions;

            _xmppClient = new Lazy<XmppClient>(() => new XmppClient(this, platform), true);
            PartyService = new PartyService(this);
            AccountPublicService = new AccountPublicService(this);
        }

        /// <summary>
        /// Logs in with an authorization code, will authenticate with device auth.
        /// </summary>
        /// <param name="code">The authorization code, if null it will get the code given in the api client builder.</param>
        /// <param name="clientToken">WebsocketClient token used for the authorization code, must be the same as the client for the code.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The device that has been created.</returns>
        public async Task<Device> LoginWithAuthorizationCodeAsync(
            string code = null,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
        {
            var codeUsed = code ?? AuthConfig.AuthorizationCode;
            if (codeUsed == null)
            {
                throw new ArgumentException("Tried to login with an authorization code, but there was no code specified in the call or the client builder.");
            }

            var authorizationAuth = await AccountPublicService
                .AuthWithAuthorizationCodeAsync(codeUsed, clientToken, cancellationToken)
                .ConfigureAwait(false);
            if (!authorizationAuth.IsSuccessful)
            {
                throw new FortniteException("Failed to authenticate with the authorization code", authorizationAuth.Error);
            }

            var exchangeCode = await AccountPublicService
                .GetExchangeAsync(authorizationAuth.Data, cancellationToken)
                .ConfigureAwait(false);
            if (!exchangeCode.IsSuccessful)
            {
                throw new FortniteException("Failed to get the exchange code.", exchangeCode.Error);
            }

            var exchangeAuth =
                await LoginWithExchangeAsync(exchangeCode.Data, cancellationToken: cancellationToken);

            var deviceResponse = await AccountPublicService
                .CreateDeviceAsync(exchangeAuth, cancellationToken)
                .ConfigureAwait(false);
            if (!deviceResponse.IsSuccessful)
            {
                throw new FortniteException("Failed to create a device.", deviceResponse.Error);
            }

            await LoginWithDeviceAsync(deviceResponse.Data, cancellationToken: cancellationToken);
            return deviceResponse.Data;
        }

        /// <summary>
        /// Logs in with the exchange code.
        /// </summary>
        /// <param name="exchangeCode">Exchange code</param>
        /// <param name="clientToken">WebsocketClient token, default is the client token specified in the client builder/</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Authentication response</returns>
        public async Task<AuthResponse> LoginWithExchangeAsync(
            ExchangeCode exchangeCode,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
        {
            if (exchangeCode == null)
            {
                throw new ArgumentException("Tried to login with exchange code, but there was no exchange specified in the call.");
            }

            return await LoginWithExchangeAsync(exchangeCode.Code, clientToken, cancellationToken);
        }

        /// <inheritdoc cref="LoginWithExchangeAsync(Fortnite.Net.Objects.Auth.ExchangeCode,Fortnite.Net.ClientToken,System.Threading.CancellationToken)"/>
        public async Task<AuthResponse> LoginWithExchangeAsync(
            string exchangeCode = null,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
        {
            var exchangeCodeUsed = exchangeCode ?? AuthConfig.ExchangeCode;
            if(exchangeCodeUsed == null)
            {
                throw new ArgumentException("Tried to login with exchange code, but there was no exchange specified in the call or the client builder.");
            }

            var response = await AccountPublicService
                .AuthWithExchangeAsync(exchangeCodeUsed, clientToken, cancellationToken)
                .ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                throw new FortniteException("Failed to authenticate with the exchange code.", response.Error);
            }

            var responseData = response.Data;
            await OnLoginAsync(responseData);

            return responseData;
        }

        /// <summary>
        /// Logs in with the device.
        /// </summary>
        /// <param name="device">Device</param>
        /// <param name="clientToken">WebsocketClient token, default is the one specified in the client builder.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<Device> LoginWithDeviceAsync(
            Device device = null,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
        {
            var deviceUsed = device ?? AuthConfig.Device;
            if (deviceUsed == null)
            {
                throw new ArgumentException("Tried to login with device, but there was no device specified in the call or the client builder.");
            }

            await LoginWithDeviceAsync(deviceUsed.AccountId, deviceUsed.DeviceId, deviceUsed.Secret, clientToken, cancellationToken)
                .ConfigureAwait(false);

            return device;
        }

        /// <inheritdoc cref="LoginWithDeviceAsync(Fortnite.Net.Objects.Auth.Device,Fortnite.Net.ClientToken,System.Threading.CancellationToken)"/>
        /// <param name="accountId">Id of the account</param>
        /// <param name="deviceId">Id of the device</param>
        /// <param name="secret">Secret</param>
        /// <param name="clientToken">WebsocketClient token, default is the one specified in the client builder.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<AuthResponse> LoginWithDeviceAsync(
            string accountId,
            string deviceId,
            string secret,
            ClientToken clientToken = null,
            CancellationToken cancellationToken = default)
        {
            var response = await AccountPublicService
                .AuthWithDeviceAsync(accountId, deviceId, secret, clientToken, cancellationToken)
                .ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                throw new FortniteException("Failed to authenticate with device.", response.Error);
            }

            var responseData = response.Data;
            await OnLoginAsync(responseData);

            return responseData;
        }

        internal RestClient CreateRestClient(BaseService service)
        {
            var restClient = new RestClient(service.BaseUrl);
            _restClientAction?.Invoke(restClient);
            restClient.UseSerializer<NewtonsoftSerializer>();

            return restClient;
        }

        internal void VerifyLogin()
        {
            if (!IsLoggedIn)
            {
                throw new FortniteException("You must be logged in to use this service.");
            }
        }

        internal async Task OnLoginAsync(AuthResponse authResponse)
        {
            if (Login != null)
            {
                await Login?.Invoke(authResponse);
            }

            CurrentLogin = authResponse;
            IsLoggedIn = true;
        }

        private bool _disposed;

        /// <summary>
        /// Kills the current session, if there are too many active sessions
        /// you may not be able to use epic's services for a few hours.
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            await AccountPublicService.KillCurrentSessionAsync();
            _disposed = true;
        }

    }
}
