using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class EmployeeMapping : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);

            builder.Property("Name")
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property("Email")
                   .HasMaxLength(255)
                   .IsRequired(false);
        }
    }
}
