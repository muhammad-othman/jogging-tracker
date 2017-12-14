using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using JoggingTrackerServer.Persistence.Shared;
using JoggingTrackerServer.Domain.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using JoggingTrackerServer.Persistence.Repositories;
using System.Linq;
using FluentAssertions;
using JoggingTrackerServer.Domain.Enums;

namespace JoggingTrackerServer.Persistence.Test
{
    [TestClass]
    public class JogsRepositoryTests
    {
        private JogRepository joggingRepo;
        private Mock<DbSet<Jog>> mockJoggings;
        private List<Jog> data;
        [TestInitialize]
        public void TestInit()
        {
            data = new List<Jog>();

            var admin = new User { ID = 1, UserName = "admin", Password = "admin123", Permission = UserPermission.Admin, Email = "admin@jogging.com", Age = 50, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now };
            var manager = new User { ID = 2, UserName = "manager", Password = "manager123", Permission = UserPermission.Manager, Email = "manager@jogging.com", Age = 30, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now };
            var user = new User { ID = 3, UserName = "firstuser", Password = "firstuser", Permission = UserPermission.Regular, Email = "firstuser@jogging.com", Age = 20, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now };
            var user2 = new User { ID = 4, UserName = "seconduser", Password = "seconduser", Permission = UserPermission.Regular, Email = "seconduser@jogging.com", Age = 40, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now };

            data.Add(new Jog {ID = 1 , Date = DateTime.Now.AddDays(-33), Distance = 5000, Duration = 60, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 2 , Date = DateTime.Now.AddDays(-30), Distance = 7000, Duration = 60, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 3 , Date = DateTime.Now.AddDays(-25), Distance = 3000, Duration = 40, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 4 , Date = DateTime.Now.AddDays(-24), Distance = 10000, Duration = 80, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 5, Date = DateTime.Now.AddDays(-22), Distance = 4000, Duration = 30, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 6 , Date = DateTime.Now.AddDays(-19), Distance = 5000, Duration = 60, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 7 , Date = DateTime.Now.AddDays(-15), Distance = 9000, Duration = 80, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 8 , Date = DateTime.Now.AddDays(-13), Distance = 15000, Duration = 120, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 9 , Date = DateTime.Now.AddDays(-9), Distance = 3000, Duration = 30, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 10, Date = DateTime.Now.AddDays(-5), Distance = 4000, Duration = 50, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 11, Date = DateTime.Now.AddDays(-2), Distance = 5000, Duration = 50, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 12, Date = DateTime.Now.AddDays(0), Distance = 7000, Duration = 60, User = user,UserID = 3, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 13, Date = DateTime.Now.AddDays(-33), Distance = 5500, Duration = 60, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 14, Date = DateTime.Now.AddDays(-30), Distance = 7200, Duration = 60, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 15, Date = DateTime.Now.AddDays(-25), Distance = 3900, Duration = 40, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 16, Date = DateTime.Now.AddDays(-24), Distance = 14000, Duration = 80, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 17, Date = DateTime.Now.AddDays(-22), Distance = 5900, Duration = 30, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 18, Date = DateTime.Now.AddDays(-19), Distance = 5400, Duration = 60, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 19, Date = DateTime.Now.AddDays(-15), Distance = 9500, Duration = 80, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 20, Date = DateTime.Now.AddDays(-13), Distance = 19000, Duration = 120, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 21, Date = DateTime.Now.AddDays(-9), Distance = 3000, Duration = 30, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 22, Date = DateTime.Now.AddDays(-5), Distance = 4000, Duration = 50, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 23, Date = DateTime.Now.AddDays(-4), Distance = 5000, Duration = 50, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 24, Date = DateTime.Now.AddDays(-3), Distance = 7000, Duration = 60, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 25, Date = DateTime.Now.AddDays(-2), Distance = 4000, Duration = 50, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 26, Date = DateTime.Now.AddDays(-2), Distance = 5000, Duration = 50, User = user2 ,UserID = 4, DateCreated = DateTime.Now });
            data.Add(new Jog {ID = 27, Date = DateTime.Now.AddDays(0), Distance = 7000, Duration = 60, User = user2 ,UserID = 4, DateCreated = DateTime.Now });

            mockJoggings = new Mock<DbSet<Jog>>();
            var mockDBContext = new Mock<IJoggingDBContext>();
            mockDBContext.SetupGet(e => e.Jogs).Returns(new MockDbSet<Jog>(data));
            joggingRepo = new JogRepository(mockDBContext.Object);
        }
        
        [TestMethod]
        public void GetAllJoggings_ShouldReturnAllJoggins()
        {
            var jogs = joggingRepo.GetAll().ToList();
            Assert.AreEqual(jogs[0], data[0]);
            Assert.AreEqual(jogs[1], data[1]);
            Assert.AreEqual(jogs[2], data[2]);
        }
        [TestMethod]
        public void GetJoggingByID_ShouldReturnTheRightJogging()
        {
            var jog = joggingRepo.GetByID(5);
            Assert.AreEqual(jog, data[4]);
        }
        [TestMethod]
        public void GetJoggingDate_ShouldReturnTheRightJoggings()
        {
            var jogs = joggingRepo.GetJogging(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)).ToList();
            Assert.AreEqual(jogs.Count(), 2);
            Assert.AreEqual(jogs[0], data[11]);
            Assert.AreEqual(jogs[1], data[26]);
        }
        [TestMethod]
        public void GetUserJogging_ShouldReturnTheRightJoggings()
        {
            var jogs = joggingRepo.GetUserJogging(3,null,null).ToList();
            Assert.AreEqual(jogs.Count(), 12);
            Assert.AreEqual(jogs[0], data[0]);
            Assert.AreEqual(jogs[1], data[1]);
            Assert.AreEqual(jogs[2], data[2]);
        }
    }
}
