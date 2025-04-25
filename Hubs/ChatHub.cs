using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using WhatsApp.Events;
using WhatsApp.Hubs;
using WhatsApp.Models;
using WhatsApp.Services;

namespace WhatsApp.Hubs
{
    public class ChatHub(State _state, ChatRegistry _registry):Hub
    {
        public async Task<List<OutputMessage>> JoinRoom(RoomRequest request)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, request.Room);

            return _registry.GetMessages(request.Room)
                .Select(m => m.Output)
                .ToList();
        }

        public Task LeaveRoom(RoomRequest request)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, request.Room);
        }

        public Task SendMessage(InputMessage message)
        {
            var username = Context.User.Claims.FirstOrDefault(x => x.Type == "username").Value;

            var userMessage = new UserMessage(
                new(Context.UserIdentifier, username),
                message.Message,
                message.Room,
                DateTimeOffset.Now
            );

            _registry.AddMessage(message.Room, userMessage);
            return Clients.GroupExcept(message.Room, new[] { Context.ConnectionId })
                .SendAsync("send_message", userMessage.Output);
        
        
        }

        public IEnumerable<int> GetSquares() => _state.Squares;

        public Task SendDragEvent(DragEvent dEvent)
        {
            var username = Context.User?.Claims.FirstOrDefault(x => x.Type == "userid")?.Value ?? "barry";
            var colour = Context.User?.Claims.FirstOrDefault(x => x.Type == "color")?.Value ?? "#ff0000";
            return Clients.Others.SendAsync("on_drag", new UserDragEvent(dEvent, username, colour));
        }

        public async Task<IEnumerable<int>> EndDrag(DragEvent dEvent)
        {
            var username = Context.User?.Claims.FirstOrDefault(x => x.Type == "userid")?.Value ?? "barry";
            _state.Move(dEvent.OriginalPos, dEvent.CurrentPos);
            await Clients.Others.SendAsync("end_drag", new { _state.Squares, Username = username });
            return _state.Squares;
        }
    }

  

}



    
