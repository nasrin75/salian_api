using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class EquipmentMapping : IEntityTypeConfiguration<EquipmentEntity>
    {
        public void Configure(EntityTypeBuilder<EquipmentEntity> builder)
        {
            builder.ToTable("Equipments");
            builder.HasKey(x => x.Id);

            builder.Property("Name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property("Type")
                .IsRequired();

            builder.Property("IsShowInMenu")
               .HasDefaultValue(false);

            builder.Property("DeletedAt")
                .IsRequired(false);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
