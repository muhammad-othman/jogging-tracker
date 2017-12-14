using System;

namespace JoggingTrackerServer.Domain.Entities
{
    public interface Entity
    {
        int ID { get; set; }
        DateTime DateCreated { get; set; }
    }
}