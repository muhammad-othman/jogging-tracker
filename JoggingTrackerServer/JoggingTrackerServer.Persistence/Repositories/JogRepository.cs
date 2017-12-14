using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.IRepositories;
using JoggingTrackerServer.Persistence.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace JoggingTrackerServer.Persistence.Repositories
{
    public class JogRepository : Repository<Jog>, IJogRepository
    {
        public override Jog GetByID(int ID)
        {
            return _dbContext.Jogs.Where(e => e.ID == ID).Include(e => e.User).FirstOrDefault();
        }
        public override IEnumerable<Jog> GetAll()
        {
            return _dbContext.Jogs.Include(e => e.User).ToList();
        }
        public JogRepository(IJoggingDBContext database) : base(database)
        {

        }
        public IEnumerable<Jog> GetJogging(DateTime? From, DateTime? To)
        {
            if (From == null || To == null)
                return GetAll();
            return _dbContext.Jogs.Where(j => j.Date >= From && j.Date <= To).Include(e => e.User).ToList();
        }
        public IEnumerable<Jog> GetUserJogging(int UserID, DateTime? From, DateTime? To)
        {
            if (From == null || To == null)
                return _dbContext.Jogs.Where(j => j.UserID == UserID).Include(e => e.User).ToList();
            return _dbContext.Jogs.Where(j => j.UserID == UserID && j.Date >= From && j.Date <= To).Include(e => e.User).ToList();
        }
    }
}
