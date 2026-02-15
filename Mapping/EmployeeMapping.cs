using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class EmployeeMapping : IEntityTypeConfiguration<EmployeeEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);

            builder.Property("Name").HasMaxLength(255).IsRequired();

            builder.Property("Email").HasMaxLength(255).IsRequired(false);

            builder.Property("DeletedAt").IsRequired(false);

            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
