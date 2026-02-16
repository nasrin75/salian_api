using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class PermissionMapping : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(x => x.Id);

            builder.Property("Name").HasMaxLength(100).IsRequired();
            builder.Property("Title").HasMaxLength(250);
            builder.Property("Category").HasMaxLength(250);

            builder.Property("DeletedAt").IsRequired(false);

            builder.HasQueryFilter(r => r.DeletedAt == null);
        }
    }
}
