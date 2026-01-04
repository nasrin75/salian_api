using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class RoleMapping : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);

            builder.Property("FaName")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property("EnName")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property("IsDeleted")
               .HasDefaultValue(false);

            builder.HasQueryFilter(r => !r.IsDeleted);

            // define relation
            builder.HasMany(u => u.Users)
                .WithOne(r => r.Role)
                .HasForeignKey(r => r.RoleId);
        }
    }
}
