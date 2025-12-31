using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) {

            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property("Username")
                    .HasMaxLength(255)
                    .IsRequired();

            builder.Property("Password")
                    .HasMaxLength(255)
                    .IsRequired();

            builder.Property("Email")
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property("Mobile")
                   .HasMaxLength(11)
                   .IsRequired(false);

            builder.Property("IsCheckIp")
                   .HasDefaultValue(false);

            builder.Property("LoginType")
                   .HasDefaultValue("1");

            // define relation
            builder.HasMany(e => e.IpWhiteLists)
                .WithOne(e => e.User)
                .HasForeignKey(e=>e.Id);

        }
    }
}
