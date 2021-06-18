namespace Fortnite.Net.Enums
{
    public enum RefreshType
    {

        /// <summary>
        /// This will start a scheduler every x hours.
        /// </summary>
        Scheduler,

        /// <summary>
        /// This will refresh when the epic api is called and the access token has expired.
        /// </summary>
        OnCall,

        /// <summary>
        /// No refresh
        /// </summary>
        None

    }
}
