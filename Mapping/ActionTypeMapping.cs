using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class ActionTypeMapping : IEntityTypeConfiguration<ActionTypeEntity>
    {
        public void Configure(EntityTypeBuilder<ActionTypeEntity> builder)
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

            builder.Property("DeletedAt")
               .IsRequired(false);

            builder.HasQueryFilter(r => r.DeletedAt == null);
        }
    }
}
