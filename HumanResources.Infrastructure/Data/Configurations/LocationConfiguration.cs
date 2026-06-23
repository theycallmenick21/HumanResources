using HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HumanResources.Infrastructure.Data.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id).HasColumnName("id");
            builder.Property(r => r.Address).HasColumnName("address");
            builder.Property(r => r.CityId).HasColumnName("city_id");

            builder.HasOne(r => r.City)
                   .WithMany()
                   .HasForeignKey(r => r.CityId);
        }
    }
}
