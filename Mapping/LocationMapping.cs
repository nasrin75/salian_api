using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class LocationMapping :IEntityTypeConfiguration<LocationEntity>
    {
        public void Configure(EntityTypeBuilder<LocationEntity> builder)
        {
            builder.ToTable("Locations");
            builder.HasKey(x => x.Id);

            builder.Property("Title")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property("Abbreviation")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property("IsShow")
               .HasDefaultValue(true);

            builder.Property("DeletedAt")
              .IsRequired(false);

            builder.HasQueryFilter(r => r.DeletedAt == null);

            /* builder.HasOne(x => x.Employee)
                 .WithOne(x => x.Location)
                 .HasForeignKey<EmployeeEntity>(x=>x.LocationId);*/

        }
    }
}
