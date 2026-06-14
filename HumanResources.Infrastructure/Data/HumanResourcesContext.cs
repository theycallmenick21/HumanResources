using HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Infrastructure.Data
{
    public class HumanResourcesContext : DbContext
    {
        public DbSet<Country> Paises { get; set; }

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
