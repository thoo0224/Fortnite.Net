using Fortnite.Net.Attributes;
using Fortnite.Net.Exceptions;
using Fortnite.Net.Objects.Party;
using Fortnite.Net.Xmpp.Events;

using System.Linq;
using System.Threading.Tasks;

namespace Fortnite.Net.Xmpp.Notifications
{
    public class PartyNotifications : INotifications
    {

        private readonly XmppClient _client;

        public PartyNotifications(XmppClient client)
        {
            _client = client;
        }

        [XmppNotification("com.epicgames.social.party.notification.v0.PING", typeof(PingEvent))]
        public async Task Ping(PingEvent e)
        {
            await _client.OnPingAsync(e);

            var parties = await _client.Client.PartyService.GetPingsAsync(e.PingerId);
            var party = parties.Data.FirstOrDefault();
            if(party == null)
            {
                throw new FortniteException($"Couldn't find invite from {e.PingerId}");
            }

            var invite = party.Invites.FirstOrDefault(x => x.SentBy.Equals(e.PingerId) && x.Status.Equals("SENT")) ?? new PartyInvitation
            {
                PartyId = party.Id,
                SentBy = e.PingerId,
                Meta = party.Meta,
                SentTo = _client.Client.CurrentLogin.AccountId,
                SentAt = e.Sent,
                UpdatedAt = e.Sent,
                ExpiresAt = e.Expires,
                Status = "SENT"
            };
            invite.Client = _client;

            await _client.OnPartyInvitationAsync(invite);
        }

    }
}
