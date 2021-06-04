using Fortnite.Net.Enums;

using Serilog;
using Serilog.Core;
using Serilog.Events;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Fortnite.Net.Test
{
    internal class Program
    {

        public static async Task Main(string[] args)
        {
            Log.Logger = CreateLogger();

            var accountId = Environment.GetEnvironmentVariable("accountId");
            var deviceId = Environment.GetEnvironmentVariable("deviceId");
            var secret = Environment.GetEnvironmentVariable("secret");

            var client = new FortniteApiClientBuilder()
                .WithPlatform(Platform.WIN)
                .WithDefaultClientToken(ClientToken.FortniteIosGameClient)
                .Create();
            await client.LoginWithDeviceAsync(accountId, deviceId, secret, ClientToken.FortniteIosGameClient);

            Debugger.Break();
            /*RegisterEvents(client);
            var thread = new Thread(async () =>
            {
                await client.XmppClient.StartAsync();
            })
            {
                IsBackground = true
            };
            thread.Start();

            await Task.Delay(-1);*/
        }

        private static void RegisterEvents(FortniteApiClient client)
        {
            client.XmppClient.Disconnected += () =>
            {
                Log.Information("XMPP Disconnected");
                return Task.CompletedTask;
            };
            client.XmppClient.Connected += () =>
            {
                Log.Information("XMPP Connected");
                return Task.CompletedTask;
            };
            client.XmppClient.NotificationReceived += e =>
            {
                Log.Information("Received notification {Notification}", e.Type);
                return Task.CompletedTask;
            };
            client.XmppClient.Ping += e =>
            {
                Log.Information("Received ping by {Name} ({Id})", e.PingerDisplayName, e.PingerId);
                return Task.CompletedTask;
            };
            client.XmppClient.PartyInvitation += async invitation =>
            {
                Log.Information("Received party invitation from {FromId} Party: {PartyId}", invitation.SentBy, invitation.PartyId);
                try
                {
                    await invitation.AcceptAsync();
                }
                catch(Exception e)
                {
                    Log.Fatal(e, "Something went wrong accepting the party invitation: {Message}", e.Message);
                    return;
                }
                Log.Information("Successfully joined party.");
            };
            client.XmppClient.PartyChatMessageReceived += e =>
            {
                Log.Information("Received party chat message from {PartyId}. {From}: {Message}", e.Party.Id, e.From.Id, "Unknown.");
                return Task.CompletedTask;
            };
            client.XmppClient.ChatMessageReceived += e =>
            {
                Log.Information("Received chat message. {From}: {Message}", e.From, e.Message);
                return Task.CompletedTask;
            };
        }

        private static ILogger CreateLogger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", new LoggingLevelSwitch(LogEventLevel.Warning))
                .MinimumLevel.Override("System", new LoggingLevelSwitch(LogEventLevel.Warning))
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", new LoggingLevelSwitch())
                .WriteTo.Console(
                    theme: Globals.ConsoleTheme,
                    outputTemplate: "[{Timestamp:G}] [{Level:u3}] {Message:l}{NewLine}")
                .CreateLogger();
        }

    }
}
