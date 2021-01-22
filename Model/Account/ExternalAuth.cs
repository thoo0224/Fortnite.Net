using System;
using System.Collections.Generic;

namespace Fortnite.Net.Model.Account
{
    
    public class ExternalAuth
    {

        public string AccountId { get; set; }
        public string Type { get; set; }
        public AuthId[] AuthIds { get; set; }
        public string ExternalAuthId { get; set; }
        public string ExternalAuthSecondaryId { get; set; }
        public DateTime DateAdded { get; set; }
        public string ExternalDisplayName { get; set; }
        public string ExternalAuthIdType { get; set; }
        public DateTime LastLogin { get; set; }

    }
    
    public class AuthId
    {
        
        public string Id { get; set; }
        public string Type { get; set; }
        public Dictionary<string, ExternalAuth> ExternalAuths { get; set; }

    }
    
}