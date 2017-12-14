using JoggingTrackerServer.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace JoggingTrackerServer.Persistence.EntitiesConfig
{
    public class JogConfig : EntityTypeConfiguration<Jog>
    {
        public JogConfig()
        {
            this.HasKey<int>(j => j.ID);
            this.Property(j => j.Date).HasColumnType("date");
        }
    }
}
