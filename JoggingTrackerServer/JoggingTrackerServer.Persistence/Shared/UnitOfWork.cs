using JoggingTrackerServer.Domain.Interfaces;
using JoggingTrackerServer.Domain.IRepositories;

namespace JoggingTrackerServer.Persistence.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly IJoggingDBContext _dbContext;

        public UnitOfWork(IJoggingDBContext database, IJogRepository jogRepository, IUserRepository userRepository)
        {
            _dbContext = database;
            JogRepository = jogRepository;
            UserRepository = userRepository;
        }
        public IJogRepository JogRepository { get; set; }
        public IUserRepository UserRepository { get; set; }

        public void Complete()
        {
            _dbContext.Save();
        }
    }
}
