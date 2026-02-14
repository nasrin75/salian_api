using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using salian_api.Entities;
using salian_api.Routes;

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

            builder.Property("DeletedAt")
               .IsRequired(false);

            builder.HasQueryFilter(r => r.DeletedAt == null);

            // define relation
            builder.HasMany(u => u.Users)
                .WithOne(r => r.Role)
                .HasForeignKey(r => r.RoleId);
            // Many to Many Relation


            builder.HasMany(r => r.Permissions)
                .WithMany(r => r.Roles);
        }
    }
}
