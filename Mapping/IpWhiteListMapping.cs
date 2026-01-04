using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class IpWhiteListMapping :IEntityTypeConfiguration<IpWhiteListEntity>
    {
        public void Configure(EntityTypeBuilder<IpWhiteListEntity> builder) {
            builder.ToTable("IpWhiteLists");
            builder.HasKey(x => x.Id);

            builder.Property("Ip")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.IpWhiteLists)
                   .HasForeignKey(x => x.UserID);
        }
    }
}
