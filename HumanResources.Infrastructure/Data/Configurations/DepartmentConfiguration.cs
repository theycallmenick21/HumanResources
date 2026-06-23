using HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HumanResources.Infrastructure.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id).HasColumnName("id");
            builder.Property(r => r.Name).HasColumnName("name");
            builder.Property(r => r.LocationId).HasColumnName("location_id");

            builder.HasOne(r => r.Location)
                   .WithMany()
                   .HasForeignKey(r => r.LocationId);
        }
    }
}
