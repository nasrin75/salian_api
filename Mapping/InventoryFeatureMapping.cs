using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class InventoryFeatureMapping : IEntityTypeConfiguration<InventoryFeatureEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryFeatureEntity> builder)
        {
            builder.ToTable("InventoryFeatures");
            builder.HasKey(e => e.Id);

            builder.Property("Value")
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
