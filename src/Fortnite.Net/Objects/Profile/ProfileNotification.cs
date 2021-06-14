namespace Fortnite.Net.Objects.Profile
{
    public class ProfileNotification
    {

        public string Type { get; set; }
        public bool Primary { get; set; }
        public int DaysLoggedIn { get; set; }
        public ProfileNotificationItem[] Items { get; set; }

    }
}
