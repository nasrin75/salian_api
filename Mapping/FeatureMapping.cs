using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class FeatureMapping : IEntityTypeConfiguration<FeatureEntity>
    {
        public void Configure(EntityTypeBuilder<FeatureEntity> builder)
        {
            builder.ToTable("Features");
            builder.HasKey(e => e.Id);

            builder.Property("Name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property("DeletedAt")
               .IsRequired(false);

            builder.HasQueryFilter(f => f.DeletedAt == null);

        }
    }
}
