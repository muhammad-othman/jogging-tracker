using JoggingTrackerServer.Domain.Entities;
using System.Collections.Generic;

namespace JoggingTrackerServer.Domain.Interfaces
{
    public interface IRepository<T> where T : class, Entity
    {
        T GetByID(int ID);
        T Create(T Entity);
        T Update(int ID, T Entity);
        T Delete(int ID);
        IEnumerable<T> GetAll();
    }
}
