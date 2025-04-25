using WhatsApp.Models;

namespace WhatsApp.DataFiles
{
    public interface IMessageRepository
    {
        void AddGroup(Group group);
        void AddMessage(Message message);
        Task<Group> GetGroupForConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<IEnumerable<Message>> GetMessageThread(string name, string otherUser);
        void RemoveConnection(Connection connection);
    }
}