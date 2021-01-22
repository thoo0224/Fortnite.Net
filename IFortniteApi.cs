using System.Threading.Tasks;
using Fortnite.Net.Model.Account;

namespace Fortnite.Net
{
    public interface IFortniteApi
    {

        Task LoginWithDeviceAsync(Device device = null!);
        void LoginWithDevice();
        
        Task LoginWithAuthorizationCodeAsync();
        void LoginWithAuthorizationCode();
        
        Task LoginWithExchangeAsync();
        void LoginWithExchange();

    }
}