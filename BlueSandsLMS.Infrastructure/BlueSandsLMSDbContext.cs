using Microsoft.EntityFrameworkCore;
using BlueSandsLMS.Core.Entities;

namespace BlueSandsLMS.Infrastructure
{
    public class BlueSandsLMSDbContext : DbContext
    {
        public BlueSandsLMSDbContext(DbContextOptions<BlueSandsLMSDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<School> Schools => Set<School>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.Parse("ae17f104-0ec3-47e3-9517-0e7e2c3be8b0"), Name = "Student" },
                new Role { Id = Guid.Parse("d7c51101-d2a4-40d5-bb0a-bd97898cf847"), Name = "Teacher" },
                new Role { Id = Guid.Parse("0187a77f-ea11-4f8c-87b5-03d3639adfbb"), Name = "SchoolAdmin" },
                new Role { Id = Guid.Parse("83b9ce68-4195-4c10-8e08-3dd6af2b0ec9"), Name = "GlobalAdmin" },
                new Role { Id = Guid.Parse("c1aaf5f0-f1d4-4c7e-a1b3-dbb1a8c92d89"), Name = "Parent" }
            );

            // Seed School (provide ALL required fields: Subdomain, etc)
            modelBuilder.Entity<School>().HasData(
                new School
                {
                    Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                    Name = "Blue Sands Test School",
                    Subdomain = "bluesands-test"
                    // Add any more required fields here (e.g., Address = "...", City = "...", etc)
                }
            );

            // Seed Global Admin (no SchoolId assigned)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("36a6c8c6-7f46-4043-939e-382dbb42db2b"),
                    FullName = "Ifedayo Michael",
                    Email = "ifemicheal2@gmail.com",
                    PasswordHash = "$2a$11$cEhjobe.nmtMXJHMXQWhW.a7HJrFSOdBhXdqYi2Oj4BYeTh9LXC2y", // BCrypt hash for Nisotgreg0
                    RoleId = Guid.Parse("83b9ce68-4195-4c10-8e08-3dd6af2b0ec9"),
                    IsActive = true,
                    DateCreated = new DateTime(2024, 08, 03, 0, 0, 0, DateTimeKind.Utc)
                    // SchoolId = null (don't set for GlobalAdmin if allowed in your User entity)
                }
            );
        }
    }
}
