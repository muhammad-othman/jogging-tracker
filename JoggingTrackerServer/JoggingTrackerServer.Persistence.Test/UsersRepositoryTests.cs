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
    public class UsersRepositoryTests
    {
        private UserRepository usersRepo;
        private Mock<DbSet<User>> mockUsers;
        private List<User> data;
        [TestInitialize]
        public void TestInit()
        {
            data = new List<User>();

            var admin = new User {Token = "xyz", ID = 1, UserName = "admin", Password = "admin123", Permission = UserPermission.Admin, Email = "admin@jogging.com", Age = 50, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now.AddDays(5) };
            var manager = new User { Token = "abc", ID = 2, UserName = "manager", Password = "manager123", Permission = UserPermission.Manager, Email = "manager@jogging.com", Age = 30, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now.AddDays(9) };
            var user = new User { Token = "efg", ID = 3, UserName = "firstuser", Password = "firstuser", Permission = UserPermission.Regular, Email = "firstuser@jogging.com", Age = 20, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now.AddDays(-3) };
            var user2 = new User { Token = "hij", ID = 4, UserName = "seconduser", Password = "seconduser", Permission = UserPermission.Regular, Email = "seconduser@jogging.com", Age = 40, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now.AddDays(-5) };

            user.Jogs.Add(new Jog { ID = 1, Date = DateTime.Now.AddDays(-33), Distance = 5000, Duration = 60, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 2, Date = DateTime.Now.AddDays(-30), Distance = 7000, Duration = 60, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 3, Date = DateTime.Now.AddDays(-25), Distance = 3000, Duration = 40, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 4, Date = DateTime.Now.AddDays(-24), Distance = 10000, Duration = 80, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 5, Date = DateTime.Now.AddDays(-22), Distance = 4000, Duration = 30, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 6, Date = DateTime.Now.AddDays(-19), Distance = 5000, Duration = 60, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 7, Date = DateTime.Now.AddDays(-15), Distance = 9000, Duration = 80, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 8, Date = DateTime.Now.AddDays(-13), Distance = 15000, Duration = 120, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 9, Date = DateTime.Now.AddDays(-9), Distance = 3000, Duration = 30, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 10, Date = DateTime.Now.AddDays(-5), Distance = 4000, Duration = 50, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 11, Date = DateTime.Now.AddDays(-2), Distance = 5000, Duration = 50, User = user, UserID = 3, DateCreated = DateTime.Now });
            user.Jogs.Add(new Jog { ID = 12, Date = DateTime.Now.AddDays(0), Distance = 7000, Duration = 60, User = user, UserID = 3, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 13, Date = DateTime.Now.AddDays(-33), Distance = 5500, Duration = 60, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 14, Date = DateTime.Now.AddDays(-30), Distance = 7200, Duration = 60, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 15, Date = DateTime.Now.AddDays(-25), Distance = 3900, Duration = 40, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 16, Date = DateTime.Now.AddDays(-24), Distance = 14000, Duration = 80, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 17, Date = DateTime.Now.AddDays(-22), Distance = 5900, Duration = 30, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 18, Date = DateTime.Now.AddDays(-19), Distance = 5400, Duration = 60, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 19, Date = DateTime.Now.AddDays(-15), Distance = 9500, Duration = 80, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 20, Date = DateTime.Now.AddDays(-13), Distance = 19000, Duration = 120, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 21, Date = DateTime.Now.AddDays(-9), Distance = 3000, Duration = 30, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 22, Date = DateTime.Now.AddDays(-5), Distance = 4000, Duration = 50, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 23, Date = DateTime.Now.AddDays(-4), Distance = 5000, Duration = 50, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 24, Date = DateTime.Now.AddDays(-3), Distance = 7000, Duration = 60, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 25, Date = DateTime.Now.AddDays(-2), Distance = 4000, Duration = 50, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 26, Date = DateTime.Now.AddDays(-2), Distance = 5000, Duration = 50, User = user2, UserID = 4, DateCreated = DateTime.Now });
            user2.Jogs.Add(new Jog { ID = 27, Date = DateTime.Now.AddDays(0), Distance = 7000, Duration = 60, User = user2, UserID = 4, DateCreated = DateTime.Now });
            data.Add(admin);
            data.Add(manager);
            data.Add(user);
            data.Add(user2);
            mockUsers = new Mock<DbSet<User>>();
            var mockDBContext = new Mock<IJoggingDBContext>();
            mockDBContext.SetupGet(e => e.Users).Returns(new MockDbSet<User>(data));
            usersRepo = new UserRepository(mockDBContext.Object);
        }

        [TestMethod]
        public void AuthenticateUsers_ShouldReturnTheRightUser()
        {
            var user = usersRepo.AuthorizateUser("xyz");
            var user2 = usersRepo.AuthorizateUser("abc");
            Assert.AreEqual(user, data[0]);
            Assert.AreEqual(user2, data[1]);
        }
        [TestMethod]
        public void AuthenticateOldUsers_ShouldReturnTheRightUser()
        {
            var user = usersRepo.AuthorizateUser("efg");
            var user2 = usersRepo.AuthorizateUser("hij");
            Assert.AreEqual(user,null);
            Assert.AreEqual(user2, null);
        }
        
    }
}
