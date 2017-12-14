namespace JoggingTrackerServer.Persistence
{
    using Domain.Entities;
    using EntitiesConfig;
    using Shared;
    using System.Data.Entity;

    public class JoggingDBContext : DbContext, IJoggingDBContext
    {

        public JoggingDBContext()
            : base("name=JoggingDBContext")
        {
            Database.SetInitializer(new JoggingDBInitializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new JogConfig());
            modelBuilder.Configurations.Add(new UserConfig());
        }

        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Jog> Jogs { get; set; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public void Save()
        {
            this.SaveChanges();
        }
    }
}