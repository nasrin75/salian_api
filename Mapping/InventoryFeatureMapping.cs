using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class InventoryFeatureMapping : IEntityTypeConfiguration<InventoryFeature>
    {
        public void Configure(EntityTypeBuilder<InventoryFeature> builder)
        {
            builder.ToTable("InventoryFeatures");
            builder.HasKey(e => e.Id);

            builder.Property("Value")
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
