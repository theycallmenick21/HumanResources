using HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Infrastructure.Data
{
    public class HumanResourcesContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Server=acela.proxy.rlwy.net;Port=43625;Database=railway;User=root;Password=KMagdtBvQTWbpTaPRHSufREJEbECGjKh;";

                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }
    }
}
