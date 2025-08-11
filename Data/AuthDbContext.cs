using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NivelaService.Data
{
    public class AuthDbContext : IdentityDbContext
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "0f8fad5b-d9cb-469f-a165-70867728950e";
            var writeRoleId = "7c9e6679-7425-40de-944b-e07fc1f90ae7";

            // create writer and reader roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writeRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writeRoleId
                }
            };
            // Seed roles
            builder.Entity<IdentityRole>().HasData(roles);

            //create admin user 
            var adminUserId = "3d813cbb-47fb-32ba-91df-831e1593ac29";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@mydiary.com",
                Email = "admin@mydiary.com",
                NormalizedEmail = "admin@mydiary.com".ToUpper(),
                NormalizedUserName = "admin@mydiary.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@m23");

            builder.Entity<IdentityUser>().HasData(admin);

            // Give roles to Admin

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                 new()
                {
                    UserId = adminUserId,
                    RoleId = writeRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }

}
}
