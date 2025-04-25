
using WhatsApp.Models;

namespace WhatsApp.DataFiles
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string? username);
    }
}