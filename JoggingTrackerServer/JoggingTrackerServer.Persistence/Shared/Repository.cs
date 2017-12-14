using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoggingTrackerServer.Persistence.Shared
{
    public class Repository<T> : IRepository<T> where T : class, Entity
    {
        protected readonly IJoggingDBContext _dbContext;

        public Repository(IJoggingDBContext database)
        {
            _dbContext = database;
        }
        public virtual T Create(T Entity)
        {
            Entity.DateCreated = DateTime.Now;
            return _dbContext.Set<T>().Add(Entity);
        }

        public virtual T Delete(int ID)
        {
            var entity = _dbContext.Set<T>().SingleOrDefault(e => e.ID == ID);
            if (entity != null)
                _dbContext.Set<T>().Remove(entity);
            return entity;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public virtual T GetByID(int ID)
        {
            return _dbContext.Set<T>().SingleOrDefault(e => e.ID == ID);
        }

        public virtual T Update(int ID, T Entity)
        {
            var oldEntity = _dbContext.Set<T>().Find(ID);
            if (oldEntity == null)
            {
                return null;
            }
            Entity.ID = oldEntity.ID;
            Entity.DateCreated = oldEntity.DateCreated;
            _dbContext.Entry(oldEntity).CurrentValues.SetValues(Entity);
            Entity.ID = oldEntity.ID;
            return Entity;
        }
    }
}
