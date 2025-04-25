
using WhatsApp.Models;

namespace WhatsApp.DataFiles
{
    public class UserRepository : IUserRepository
    {
        public Task<User> GetUserByUsernameAsync(string? username)
        {
            throw new NotImplementedException();
        }
    }
}
