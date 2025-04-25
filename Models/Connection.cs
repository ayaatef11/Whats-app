
namespace WhatsApp.Models
{
    public class Connection
    {
        public string ConnectionId{get;set;}
        public string UserName { get; internal set; }

        public  string Name;

        public Connection(string connectionId, string? name)
        {
            ConnectionId = connectionId;
            Name = name;
        }

      
    }
}
