using Fortnite.Net.Config;
using Fortnite.Net.Enums;
using Fortnite.Net.Objects.Auth;

using RestSharp;

using System;

namespace Fortnite.Net
{
    /// <summary>
    /// Used to create the fortnite api client.
    /// </summary>
    public class FortniteApiClientBuilder
    {

        private readonly AuthConfig _authConfig = new AuthConfig();

        private Action<RestClient> _restClientAction;
        private string _userAgent;
        private ClientToken _clientToken;
        private Platform _platform = Platform.WIN;

        /// <summary>
        /// Sets the default user agent of the http client.
        /// </summary>
        /// <param name="userAgent">User agent</param>
        /// <returns></returns>
        public FortniteApiClientBuilder WithUserAgent(string userAgent)
        {
            _userAgent = userAgent;
            return this;
        }

        /// <summary>
        /// Sets the authorization code.
        /// </summary>
        /// <param name="authorizationCode">Authorization code</param>
        /// <returns>Client builder</returns>
        public FortniteApiClientBuilder WithAuthorizationCode(string authorizationCode)
        {
            _authConfig.AuthorizationCode = authorizationCode;
            return this;
        } 

        /// <summary>
        /// Configures the rest client.
        /// </summary>
        /// <param name="restClientAction">Action for the rest client</param>
        /// <returns>Client builder</returns>
        public FortniteApiClientBuilder ConfigureRestClient(Action<RestClient> restClientAction)
        {
            _restClientAction = restClientAction;
            return this;
        }

        /// <summary>
        /// Sets the client token for the fortnite client. (The default client token is <see cref="ClientToken.FortniteIosGameClient"/>
        /// </summary>
        /// <param name="clientToken">WebsocketClient token which you can get from <seealso cref="ClientToken"/> or your own. A client token is 'ClientId:Secret' encoded in base64.</param>
        /// <returns>Client builder</returns>
        public FortniteApiClientBuilder WithDefaultClientToken(ClientToken clientToken)
        {
            _clientToken = clientToken;
            return this;
        }

        /// <summary>
        /// Sets the device.
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <param name="deviceId">Id of the device</param>
        /// <param name="secret">Secret</param>
        /// <returns>Client builder</returns>
        public FortniteApiClientBuilder WithDevice(
            string accountId,
            string deviceId,
            string secret)
        {
            return WithDevice(new Device(accountId, deviceId, secret));
        }

        /// <inheritdoc cref="WithDevice(string,string,string)"/>
        /// <param name="device">Device</param>
        public FortniteApiClientBuilder WithDevice(Device device)
        {
            _authConfig.Device = device;
            return this;
        }

        /// <summary>
        /// Sets the platform for the presence of parties. Default platform is WIN.
        /// </summary>
        /// <param name="platform">Platform</param>
        /// <returns>Client builder</returns>
        public FortniteApiClientBuilder WithPlatform(Platform platform)
        {
            _platform = platform;
            return this;
        }

        public FortniteApiClientBuilder WithAutoRefresh(bool autoRefresh)
        {
            _authConfig.AutoRefresh = autoRefresh;
            return this;
        }

        /// <summary>
        /// Creates the api client.
        /// </summary>
        /// <returns>Api client</returns>
        public FortniteApiClient Create()
        {
            return new FortniteApiClient(
                _authConfig,
                _restClientAction,
                _userAgent,
                _clientToken ?? ClientToken.FortniteIosGameClient,
                _platform);
        }

    }
}
