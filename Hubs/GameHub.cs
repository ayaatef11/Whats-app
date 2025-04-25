using Microsoft.AspNetCore.SignalR;
using WhatsApp.Interfaces;
using WhatsApp.Models;

namespace WhatsApp.Hubs
{
        public class GameHub : Hub<IGameClient>
        {
            private readonly RoomManager _manager;

            public GameHub(RoomManager manager)
            {
                _manager = manager;
            }

            public async Task<RoomView> Create()
            {
                var room = new Room { Id = Guid.NewGuid().ToString() };
                room.Users.Add(Context.UserIdentifier);
                _manager.Rooms.Add(room);

                await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

                await Clients.All.UpdateRooms();

                return new(room, Context.UserIdentifier);
            }

            public async Task<RoomView> Join(RoomRequest request)
            {
                var room = _manager.Rooms.FirstOrDefault(x => x.Id == request.Room);
                if (room == null)
                {
                    return null;
                }

                var userId = Context.UserIdentifier;

                if (room.Users.All(x => x != userId))
                {
                    room.Users.Add(userId);
                }

                await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

                return new(room, userId);
            }

            public async Task<bool> Leave()
            {
                var room = _manager.GetRoomByUserId(Context.UserIdentifier);

                if (room != null)
                {
                    room.Users.Remove(Context.UserIdentifier);
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id);
                    return true;
                }

                return false;
            }

            public Task Draw(DrawEvent drawEvent)
            {
                var room = _manager.GetRoomByUserId(Context.UserIdentifier);
                room.DrawEvents.Add(drawEvent);
                return Clients.GroupExcept(room.Id, new[] { Context.ConnectionId }).Draw(drawEvent);
            }

            public Task ClearCanvas()
            {
                var room = _manager.GetRoomByUserId(Context.UserIdentifier);
                room.DrawEvents.Clear();
                return Clients.GroupExcept(room.Id, new[] { Context.ConnectionId }).ClearCanvas();
            }

            public async Task ReDraw()
            {
                var room = _manager.GetRoomByUserId(Context.UserIdentifier);
                foreach (var drawEvent in room.DrawEvents)
                {
                    await Clients.Caller.Draw(drawEvent);
                }
            }
        }

    }

