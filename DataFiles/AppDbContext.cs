using Microsoft.EntityFrameworkCore;
using WhatsApp.Models;

namespace WhatsApp.DataFiles
{
    public class AppDbContext:DbContext
    {
        public DbSet<Group> Groups { get;  set; }
        public DbSet<Message> Messages { get; internal set; }
        public DbSet<Connection> Connections { get; internal set; }
    }
}
