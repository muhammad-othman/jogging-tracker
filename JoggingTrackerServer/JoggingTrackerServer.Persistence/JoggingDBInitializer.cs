using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.Enums;
using System;
using System.Data.Entity;

namespace JoggingTrackerServer.Persistence
{
    internal class JoggingDBInitializer : CreateDatabaseIfNotExists<JoggingDBContext>
    {
        protected override void Seed(JoggingDBContext context)
        {
            var admin = context.Users.Add(new User { UserName = "admin", Password = "admin123", Permission = UserPermission.Admin, Email = "admin@jogging.com", Age = 50, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now });
            var manager = context.Users.Add(new User { UserName = "manager", Password = "manager123", Permission = UserPermission.Manager, Email = "manager@jogging.com", Age = 30, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now });
            var user = context.Users.Add(new User { UserName = "firstuser", Password = "firstuser", Permission = UserPermission.Regular, Email = "firstuser@jogging.com", Age = 20, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now });
            var user2 = context.Users.Add(new User { UserName = "seconduser", Password = "seconduser", Permission = UserPermission.Regular, Email = "seconduser@jogging.com", Age = 40, DateCreated = DateTime.Now, TokenExpiration = DateTime.Now });
            context.SaveChanges();

            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-33), Distance = 5000, Duration = 60, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-30), Distance = 7000, Duration = 60, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-25), Distance = 3000, Duration = 40, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-24), Distance = 10000, Duration = 80, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-22), Distance = 4000, Duration = 30, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-19), Distance = 5000, Duration = 60, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-15), Distance = 9000, Duration = 80, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-13), Distance = 15000, Duration = 120, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-9), Distance = 3000, Duration = 30, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-5), Distance = 4000, Duration = 50, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-2), Distance = 5000, Duration = 50, User = user, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(0), Distance = 7000, Duration = 60, User = user, DateCreated = DateTime.Now });

            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-33), Distance = 5500, Duration = 60, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-30), Distance = 7200, Duration = 60, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-25), Distance = 3900, Duration = 40, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-24), Distance = 14000, Duration = 80, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-22), Distance = 5900, Duration = 30, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-19), Distance = 5400, Duration = 60, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-15), Distance = 9500, Duration = 80, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-13), Distance = 19000, Duration = 120, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-9), Distance = 3000, Duration = 30, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-5), Distance = 4000, Duration = 50, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-4), Distance = 5000, Duration = 50, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-3), Distance = 7000, Duration = 60, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-2), Distance = 4000, Duration = 50, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(-2), Distance = 5000, Duration = 50, User = user2, DateCreated = DateTime.Now });
            context.Jogs.Add(new Jog { Date = DateTime.Now.AddDays(0), Distance = 7000, Duration = 60, User = user2, DateCreated = DateTime.Now });
            context.SaveChanges();
            base.Seed(context);
        }
    }
}