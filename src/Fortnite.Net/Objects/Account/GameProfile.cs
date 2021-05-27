using System.Diagnostics;

namespace Fortnite.Net.Objects.Account
{
    [DebuggerDisplay("{" + nameof(DisplayName) + "}")]
    public class GameProfile
    {

        public string Id { get; set; }
        public string DisplayName { get; set; }

    }
}
