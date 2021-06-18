using Fortnite.Net.Enums;
using Fortnite.Net.Objects.Auth;

namespace Fortnite.Net.Config
{
    internal class AuthConfig
    {

        /// <summary>
        /// Authorization code
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// Exchange Code
        /// </summary>
        public string ExchangeCode { get; set; }

        /// <summary>
        /// A device
        /// </summary>
        public Device Device { get; set; }

        /// <summary>
        /// The refresh type
        /// </summary>
        public RefreshType RefreshType { get; set; } = RefreshType.OnCall;

    }
}
