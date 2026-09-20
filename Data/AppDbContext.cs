using Microsoft.EntityFrameworkCore;
using PortfolioBackend.Entities;

namespace PortfolioBackend.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Users> Users { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Experiences> Experiences { get; set; }
        public DbSet<Educations> Educations { get; set; }
        public DbSet<ContactMessages> ContactMessages { get; set; }
        public DbSet<Certificates> Certificates { get; set; }
        
    }
}
