using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class EquipmentFeatureMapping : IEntityTypeConfiguration<EquipmentFeatureEntity>
    {
        public void Configure(EntityTypeBuilder<EquipmentFeatureEntity> builder)
        {
            builder.ToTable("EquipmentFeatures");
            builder.HasKey(e => e.Id);

            builder.Property("IsShow")
                .HasDefaultValue(true);
        }
    }
}
