using System.Threading.Tasks;
using System.Timers;
using Fortnite.Net.Model.Account;
using Fortnite.Net.Services;
using Fortnite.Net.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

#nullable enable
namespace Fortnite.Net
{
    public sealed class FortniteApi : IFortniteApi
    {
        
        public delegate void LoginEventHandler(LoginModel model);
        public event LoginEventHandler? Login;
        
        public delegate void RefreshEventHandler(LoginModel model);
        public event RefreshEventHandler? Refresh;
        
        private readonly RestClient _restClient;
        private Timer _timer;
        
        private readonly string _clientToken;
        private readonly string _authorizationCode;
        private string _exchangeCode;

        public Device? Device { get; set; }
        public LoginModel LoginModel { get; set; }

        private AccountPublicService _accountPublicService;
        public AccountPublicService AccountPublicService
        {
            get
            {
                VerifyLogin();
                return _accountPublicService;
            }
        }
        
        private FriendsPublicService _friendsPublicService;
        public FriendsPublicService FriendsPublicService
        {
            get
            {
                VerifyLogin();
                return _friendsPublicService;
            }
        }
        
        private FortnitePublicService _fortnitePublicService;
        public FortnitePublicService FortnitePublicService
        {
            get
            {
                VerifyLogin();
                return _fortnitePublicService;
            }
        }
        
        //public XmppService XmppService { get; set; }
        
#pragma warning disable 8618
        public FortniteApi(
#pragma warning restore 8618
            string exchangeCode,
            string authorizationCode,
            Device? device,
            string clientToken)
        {
            Device = device;
            _exchangeCode = exchangeCode;
            _authorizationCode = authorizationCode;
            _clientToken = clientToken;
            _restClient = new RestClient("https://account-public-service-prod.ol.epicgames.com/");
            _restClient.UseSerializer<JsonNetSerializer>();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            Login += _ =>
            {
                _accountPublicService = new AccountPublicService(this);
                _friendsPublicService = new FriendsPublicService(this);
                _fortnitePublicService = new FortnitePublicService(this);
                //XmppService = new XmppService(this);
            };
        }

        #region LoginWithDevice
        public async Task LoginWithDeviceAsync(Device device = null!)
        {
            if (Device == null && device == null)
            {
                throw new FortniteApiException("Tried to login with device but device was null.");
            }
            Device ??= device;
            LoginModel = await AuthenticateWithDevice(Device); 
            _timer = new Timer
            {
                Interval = LoginModel.RefreshExpires
            };
            _timer.Elapsed += RefreshAccount;
            _timer.Start();
            OnLogin(LoginModel);
        }

        public void LoginWithDevice() =>
            LoginWithDeviceAsync().GetAwaiter().GetResult();
        #endregion

        #region LoginWithExchange
        public async Task LoginWithExchangeAsync()
        {
            if (_exchangeCode == null)
            {
                throw new FortniteApiException("Tried to login with exchange token but the exchange token was null.");
            }
            var exchangeAuth = await AuthenticateWithExchange();
            var device = await CreateDevice(exchangeAuth);
            await LoginWithDeviceAsync(device);
        }

        public void LoginWithExchange() =>
            LoginWithExchangeAsync().GetAwaiter().GetResult();
        #endregion

        #region LoginWithAuthorization
        public async Task LoginWithAuthorizationCodeAsync()
        {
            if (_authorizationCode == null)
            {
                throw new FortniteApiException("Tried to login with authorization code but the authorization code was null.");
            }
            var authorization = await AuthenticateWithAuthorizationCode();
            var exchange = await GetExchangeAsync(authorization);
            _exchangeCode = exchange.Code;
            await LoginWithExchangeAsync();
        }

        public void LoginWithAuthorizationCode() =>
            LoginWithAuthorizationCodeAsync().GetAwaiter().GetResult();
        #endregion

        private async void RefreshAccount(object sender, ElapsedEventArgs e)
        {
            var request = new RestRequest("/account/api/oauth/token", Method.POST);
            request.AddParameter("grant_type", "refresh_token", ParameterType.GetOrPost);
            request.AddParameter("refresh_token", LoginModel.RefreshToken, ParameterType.GetOrPost);
            request.AddHeader("Authorization", $"basic {ClientToken.FortniteIosGameClient}");
            LoginModel = await _restClient.HandleRequest<LoginModel>(request);
            OnRefresh(LoginModel);
        }

        private async Task<LoginModel> AuthenticateWithAuthorizationCode()
        {
            var request = new RestRequest("/account/api/oauth/token", Method.POST);
            request.AddParameter("grant_type", "authorization_code", ParameterType.GetOrPost);
            request.AddParameter("code", _authorizationCode, ParameterType.GetOrPost);
            request.AddHeader("Authorization", $"basic {_clientToken}");
            var response = await _restClient.HandleRequest<LoginModel>(request);
            return response;
        }

        private async Task<ExchangeCode> GetExchangeAsync(LoginModel authorizationLogin)
        {
            var request = new RestRequest("/account/api/oauth/exchange", Method.GET);
            request.AddHeader("Authorization", $"bearer {authorizationLogin.AccessToken}");
            var response = await _restClient.HandleRequest<ExchangeCode>(request);
            return response;
        }

        private async Task<LoginModel> AuthenticateWithExchange()
        {
            var request = new RestRequest("/account/api/oauth/token", Method.POST);
            request.AddParameter("grant_type", "exchange_code", ParameterType.GetOrPost);
            request.AddParameter("exchange_code", _exchangeCode, ParameterType.GetOrPost);
            request.AddHeader("Authorization", $"basic {ClientToken.FortniteIosGameClient}");
            var response = await _restClient.HandleRequest<LoginModel>(request);
            LoginModel = response;
            return response;
        }

        private async Task<Device> CreateDevice(LoginModel exchangeLogin)
        {
            var request = new RestRequest($"/account/api/public/account/{exchangeLogin.AccountId}/deviceAuth", Method.POST);
            request.AddHeader("Authorization", $"bearer {exchangeLogin.AccessToken}");
            var response = await _restClient.HandleRequest<Device>(request);
            return response;
        }

        private async Task<LoginModel> AuthenticateWithDevice(Device device)
        {
            var request = new RestRequest("/account/api/oauth/token", Method.POST);
            request.AddParameter("grant_type", "device_auth", ParameterType.GetOrPost);
            request.AddParameter("account_id", device.AccountId, ParameterType.GetOrPost);
            request.AddParameter("device_id", device.DeviceId, ParameterType.GetOrPost);
            request.AddParameter("secret", device.Secret, ParameterType.GetOrPost);
            request.AddHeader("Authorization", $"basic {ClientToken.FortniteIosGameClient}");
            var response = await _restClient.HandleRequest<LoginModel>(request);
            return response;
        }

        private void VerifyLogin()
        {
            if (LoginModel == null)
            {
                throw new FortniteApiException("You must be logged in to use services.");
            }
        }

        private void OnLogin(LoginModel login)
        {
            Login?.Invoke(login);
        }

        private void OnRefresh(LoginModel login)
        {
            Refresh?.Invoke(login);
        }

    }
}