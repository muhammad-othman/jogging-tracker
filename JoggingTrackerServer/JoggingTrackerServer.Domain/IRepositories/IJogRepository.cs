using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace JoggingTrackerServer.Domain.IRepositories
{
    public interface IJogRepository : IRepository<Jog>
    {
        IEnumerable<Jog> GetJogging(DateTime? From, DateTime? To);
        IEnumerable<Jog> GetUserJogging(int UserID, DateTime? From, DateTime? To);
    }
}
