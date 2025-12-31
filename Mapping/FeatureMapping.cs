using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class FeatureMapping : IEntityTypeConfiguration<Feature>
    {
        public void Configure(EntityTypeBuilder<Feature> builder)
        {
            builder.ToTable("Features");
            builder.HasKey(e => e.Id);

            builder.Property("Name")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
