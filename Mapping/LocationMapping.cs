using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class LocationMapping :IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
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

            builder.HasOne(x => x.Employee)
                .WithOne(x => x.Location)
                .HasForeignKey<Employee>(x=>x.LocationID);

        }
    }
}
