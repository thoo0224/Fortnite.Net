# Fortnite.Net
.NET Library to interact with Fortnite/Epic Games' HTTP and XMPP services.

### Warning
This library is not finished and can contain bugs/issues, if you found any bugs or issues. Please make an issue or send me a message on Discord. (Lmao#0001)

### Example
With the FortniteApiClientBuilder you can create an instance of the FortniteApi.
```cs
FortniteApiClient client = new FortniteApiClientBuilder()
  .WithDefaultClientToken(ClientToken.FortniteIosGameClient) // Default
  .WithUserAgent("Not Fortnite/++Fortnite+Release-17.0-CL-0000000 IOS/11.3.1")
  .WithPlatform(Platform.WIN) // Default, used for XMPP
  .WithDevice("Account ID", "Device ID", "Secret")
  .WithAutoRefresh(true) // This will start a scheduler to refresh the access token every x hours. This is disabled by default.
  .Create();
await client AuthenticateWithDeviceAsync(); // This will login with the device provided in the builder.
await client.StartRefreshScheduler(); If you want to use auto refresh, this needs to be called after authenticating.
  
// The other stuff is pretty much self explanatory.
```

### Features

- Chat
  - [x] Friends
  - [ ] Parties
  - [ ] Commands
- Services
  - [ ] AccountPublicService (75%)
  - [ ] PartyService (69%)
  - [ ] FortniteService
  - [ ] EventsService
  - [ ] CodeRedemptionService
  - [ ] ChannelsService
  - [ ] CatalogService
  - [ ] FriendsService
  - [ ] FulfillmentService
  - [ ] GroupsService
  - [ ] InteractionsService
  - [ ] LauncherService
  - [ ] LibraryService
  - [ ] LightswitchService
  - [ ] LinksService
  - [ ] PresenceService
  - [ ] UserSearchService
- XMPP (Low priority)
  - [x] Connect
  - [x] Chat
  - [ ] Friends
  - Party
    - [x] Base
    - [ ] Cosmetics
- MCP (Low priority)

### Packages
- Newtonsoft.Json ([License](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md))
- Restsharp ([License](https://github.com/restsharp/RestSharp/blob/dev/LICENSE.txt))

### Contribution
Any type of contribution is very much appreciated!