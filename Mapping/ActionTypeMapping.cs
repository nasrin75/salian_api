using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class ActionTypeMapping : IEntityTypeConfiguration<ActionType>
    {
        public void Configure(EntityTypeBuilder<ActionType> builder)
        {
            builder.ToTable("ActionTypes");
            builder.HasKey(x=> x.Id );

            builder.Property("FaName")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property("EnName")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property("IsShow")
               .HasDefaultValue(true);
        }
    }
}
