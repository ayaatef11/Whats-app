
namespace WhatsApp.DataFiles
{
    public interface IUnitOfWork
    {
        Task<bool> Complete();
        bool HasChanges();


            IUserRepository UserRepository { get; }
            IMessageRepository MessageRepository { get; }
        
        }
    }

