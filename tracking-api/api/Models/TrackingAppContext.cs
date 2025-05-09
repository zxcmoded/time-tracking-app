using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace tracking_app.Models
{
    public class TrackingAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }

        public TrackingAppDbContext(DbContextOptions<TrackingAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(u => u.Id)
                      .HasDefaultValueSql("NEWID()");

                entity.Property(u => u.Username)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(u => u.Password)
                      .HasMaxLength(100)
                      .IsRequired();
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("123456"), 
                Name = "Administrator",
                DateCreated = DateTime.UtcNow
            });

            modelBuilder.Entity<TimeLog>()
                .ToTable("TimeLogs")
                .HasOne(t => t.User)
                .WithMany(u => u.TimeLogs)
                .HasForeignKey(t => t.UserId);
        }
    }
}
