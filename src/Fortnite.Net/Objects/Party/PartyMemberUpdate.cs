using System.Collections.Generic;

namespace Fortnite.Net.Objects.Party
{
    public class PartyMemberUpdate
    {

        public List<string> Delete { get; set; }

        public Dictionary<string, object> Update { get; set; }

        public int Revision { get; set; }

        public PartyMemberUpdate(int revision, Dictionary<string, object> update = null, List<string> delete = null)
        {
            Revision = revision;
            Update = update ?? PartyMember.SchemaMeta;
            Delete = delete ?? new List<string>();
        }

    }
}
