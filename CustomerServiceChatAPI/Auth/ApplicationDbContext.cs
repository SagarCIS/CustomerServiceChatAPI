using CustomerServiceChatAPI.Helper;
using CustomerServiceChatAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceChatAPI.Auth
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
     
        public DbSet<User> UserLogin { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Category> Service { get; set; }

        public DbSet<Messages> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed an initial admin user (for testing purposes)
            modelBuilder.Entity<Admin>().HasData(new Admin
            {
                Id = 1,
                Username = "admin",
                PasswordHash = PasswordHelper.HashPassword("admin123") // Use hashed password
            });
        }
    }
}
