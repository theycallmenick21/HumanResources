using HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HumanResources.Infrastructure.Data.Configurations
{
    public class RecordConfiguration : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
            builder.ToTable("Record");

            builder.HasKey(r => new { r.EmployeeId, r.StartDate });

            builder.Property(r => r.EmployeeId).HasColumnName("employee_id");
            builder.Property(r => r.StartDate).HasColumnName("start_date");
            builder.Property(r => r.EndDate).HasColumnName("end_date");
            builder.Property(r => r.RoleId).HasColumnName("role_id");
            builder.Property(r => r.DepartmentId).HasColumnName("department_id");

            builder.HasOne(r => r.Employee)
                   .WithMany()
                   .HasForeignKey(r => r.EmployeeId);

            builder.HasOne(r => r.Role)
                   .WithMany()
                   .HasForeignKey(r => r.RoleId);

            builder.HasOne(r => r.Department)
                   .WithMany()
                   .HasForeignKey(r => r.DepartmentId);
        }
    }
}