using JoggingTrackerServer.Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace JoggingTrackerServer.Persistence.Shared
{
    public interface IJoggingDBContext
    {
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        IDbSet<Jog> Jogs { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<T> Set<T>() where T : class;
        void Save();
    }
}
