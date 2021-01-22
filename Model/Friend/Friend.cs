using System;

namespace Fortnite.Net.Model.Friend
{
    public class Friend
    {
        
        public string AccountId { get; set; }
        public string Status { get; set; }
        public string Direction { get; set; }
        public DateTime Created { get; set; }
        public bool Favorite { get; set; }
        
    }
}