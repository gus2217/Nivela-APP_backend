using Microsoft.EntityFrameworkCore;
using NivelaService.Models.Domain;

namespace NivelaService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<VendorImage> Images { get; set; }

    }
}
