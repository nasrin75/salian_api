using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class EquipmentFeatureMapping : IEntityTypeConfiguration<EquipmentFeature>
    {
        public void Configure(EntityTypeBuilder<EquipmentFeature> builder)
        {
            builder.ToTable("EquipmentFeatures");
            builder.HasKey(e => e.Id);

            builder.Property("IsShow")
                .HasDefaultValue(true);
        }
    }
}
