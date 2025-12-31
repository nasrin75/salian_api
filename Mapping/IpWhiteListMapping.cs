using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class IpWhiteListMapping :IEntityTypeConfiguration<IpWhiteList>
    {
        public void Configure(EntityTypeBuilder<IpWhiteList> builder) {
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
