using WhatsApp.Hubs;
using WhatsApp.Models;

namespace WhatsApp.Interfaces
{
    public interface IClientInterface
    {
            Task ClientHook(Data data);
        }
    }

