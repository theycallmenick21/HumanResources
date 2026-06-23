using HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Infrastructure.Data
{
    public class HumanResourcesContext : DbContext
    {
        public DbSet<City> City { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Record> Record { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Server=acela.proxy.rlwy.net;Port=43625;Database=railway;User=root;Password=KMagdtBvQTWbpTaPRHSufREJEbECGjKh;";

                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasQueryFilter(e => e.IsActive);
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}
