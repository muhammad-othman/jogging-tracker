using System;

namespace JoggingTrackerServer.Domain.Entities
{
    public class Jog : Entity
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Date { get; set; }
        public int Distance { get; set; }
        public int Duration { get; set; }
        public int UserID { get; set; }

        public User User { get; set; }
    }
}
