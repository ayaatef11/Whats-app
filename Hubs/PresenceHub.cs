using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WhatsApp.Trackers;

namespace WhatsApp.Hubs
{
        [Authorize]
        public class PresenceHub(PresenceTracker _tracker) : Hub
        {

            public override async Task OnConnectedAsync()
            {
                var isOnline = await _tracker.UserConnected(Context.User.Identity.Name, Context.ConnectionId);
                if (isOnline)
                    await Clients.Others.SendAsync("UserIsOnline", Context.User.Identity.Name);

                var currentUsers = await _tracker.GetOnlineUsers();
                await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
            }

            public override async Task OnDisconnectedAsync(Exception exception)
            {
                var isOffline = await _tracker.UserDisconnected(Context.User.Identity.Name, Context.ConnectionId);

                if (isOffline)
                    await Clients.Others.SendAsync("UserIsOffline", Context.User.Identity.Name);

                await base.OnDisconnectedAsync(exception);
            }
        }
    }

