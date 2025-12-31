using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class EquipmentMapping : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.ToTable("Equipments");
            builder.HasKey(x => x.Id);

            builder.Property("Name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property("Type")
                .IsRequired();
        }
    }
}
