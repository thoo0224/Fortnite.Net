using Fortnite.Net.Attributes;
using Fortnite.Net.Exceptions;
using Fortnite.Net.Objects.Notifications;
using Fortnite.Net.Objects.Party;

using Newtonsoft.Json;

using System;
using System.Diagnostics;
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

        [XmppNotification("com.epicgames.social.party.notification.v0.PING", typeof(PingNotificationBody))]
        public async Task Ping(PingNotificationBody body)
        {
            await _client.OnPingAsync(body);

            var parties = await _client.Client.PartyService.GetPingsAsync(body.PingerId);
            var party = parties.Data.FirstOrDefault();
            if(party == null)
            {
                throw new FortniteException($"Couldn't find invite from {body.PingerId}");
            }

            var invite = party.Invites.FirstOrDefault(x => x.SentBy.Equals(body.PingerId) && x.Status.Equals("SENT")) ?? new PartyInvitation
            {
                PartyId = party.Id,
                SentBy = body.PingerId,
                Meta = party.Meta,
                SentTo = _client.Client.CurrentLogin.AccountId,
                SentAt = body.Sent,
                UpdatedAt = body.Sent,
                ExpiresAt = body.Expires,
                Status = "SENT"
            };
            invite.Client = _client;

            await _client.OnPartyInvitationAsync(invite);
        }

        [XmppNotification("com.epicgames.social.party.notification.v0.MEMBER_JOINED", typeof(MemberJoinedNotificationBody))]
        public async Task MemberJoined(MemberJoinedNotificationBody body)
        {
            var member = new PartyMember
            {
                Id = body.AccountId,
                Role = "",
                JoinedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Meta = body.Connection.Meta
            };

            if (body.AccountId.Equals(_client.Client.CurrentLogin.AccountId))
            {
                var usedMember = _client.CurrentParty.Members.FirstOrDefault(x => x.Id?.Equals(body.AccountId) ?? false) ?? member;
                _client.CurrentParty.Members.Add(usedMember);

                await _client.Client.PartyService.UpdateMemberAsync(_client.CurrentParty, usedMember);
            }
            else
            {
                _client.CurrentParty.Members.Add(member);
            }

            await _client.CurrentParty.UpdatePresence(_client);
        }

        [XmppNotification("com.epicgames.social.party.notification.v0.MEMBER_STATE_UPDATED", typeof(MemberJoinedNotificationBody))]
        public Task MemberStateUpdated(MemberJoinedNotificationBody body)
        {
            return Task.CompletedTask;
        }

    }
}
