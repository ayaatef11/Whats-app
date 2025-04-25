using Microsoft.EntityFrameworkCore;
using WhatsApp.Models;

namespace WhatsApp.DataFiles
{

    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                .Include(g => g.Connections)
                .FirstOrDefaultAsync(g => g.Connections.Any(c => c.ConnectionId == connectionId));
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(g => g.Connections)
                .FirstOrDefaultAsync(g => g.Name == groupName);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages
                .Where(m =>
                    (m.SenderUsername.ToLower() == currentUsername.ToLower() &&
                     m.RecipientUsername.ToLower() == recipientUsername.ToLower()) ||
                    (m.SenderUsername.ToLower() == recipientUsername.ToLower() &&
                     m.RecipientUsername.ToLower() == currentUsername.ToLower()))
                .OrderBy(m => m.MessageSent)
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .ToListAsync();

            return messages;
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

    }
}
