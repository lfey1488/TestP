using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace TestP.DB
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Process> Processes { get; set; } = null!;
        public DbSet<Category> Categories{ get; set; } = null!;
        public DbSet<Subdivision> Subdivisions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings.Get("connection"));
    }
}
