using HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HumanResources.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id).HasColumnName("id");
            builder.Property(r => r.FirstName).HasColumnName("first_name");
            builder.Property(r => r.LastName).HasColumnName("last_name");
            builder.Property(r => r.Email).HasColumnName("email");
            builder.Property(r => r.HiringDate).HasColumnName("hiring_date");
            builder.Property(r => r.Salary).HasColumnName("salary");
            builder.Property(r => r.RoleId).HasColumnName("role_id");
            builder.Property(r => r.DepartmentId).HasColumnName("department_id");

            builder.HasOne(r => r.Role)
                   .WithMany()
                   .HasForeignKey(r => r.RoleId);

            builder.HasOne(r => r.Department)
                   .WithMany()
                   .HasForeignKey(r => r.DepartmentId);
        }
    }
}
