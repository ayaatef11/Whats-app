using WhatsApp.Models;

namespace WhatsApp.Interfaces
{
    public interface IGameClient
    {
            Task UpdateRooms();
            Task Draw(DrawEvent e);
            Task ClearCanvas();
    }
}

