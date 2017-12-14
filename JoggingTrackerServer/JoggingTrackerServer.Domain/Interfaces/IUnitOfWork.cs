using JoggingTrackerServer.Domain.IRepositories;

namespace JoggingTrackerServer.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        void Complete();
        IJogRepository JogRepository { get; set; }
        IUserRepository UserRepository { get; set; }
    }
}
