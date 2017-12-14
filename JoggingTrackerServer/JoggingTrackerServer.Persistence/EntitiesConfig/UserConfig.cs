using JoggingTrackerServer.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace JoggingTrackerServer.Persistence.EntitiesConfig
{
    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            this.HasKey<int>(u => u.ID);

            this.HasMany(u => u.Jogs)
                .WithRequired(j => j.User)
                .HasForeignKey(e => e.UserID)
                .WillCascadeOnDelete();

            this.Property(u => u.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            this.Property(u => u.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();
            this.Property(u => u.UserName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

        }
    }
}
