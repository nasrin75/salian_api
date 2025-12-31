using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);

            builder.Property("FaName")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property("EnName")
                .HasMaxLength(100)
                .IsRequired(false);
        }
    }
}
