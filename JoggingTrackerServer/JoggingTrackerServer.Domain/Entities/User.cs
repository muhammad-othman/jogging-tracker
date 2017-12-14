using JoggingTrackerServer.Domain.Enums;
using System;
using System.Collections.Generic;

namespace JoggingTrackerServer.Domain.Entities
{
    public class User : Entity
    {
        public User()
        {
            Jogs = new List<Jog>();
        }
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public UserPermission Permission { get; set; }
        public ICollection<Jog> Jogs { get; set; }
    }
}