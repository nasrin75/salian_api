using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder) {

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
                   .HasDefaultValue(LoginTypes.password);

            builder.Property("Status")
                  .HasDefaultValue(StatusLists.deactive);

            builder.Property("IsDeleted")
                .HasDefaultValue(false);

            builder.HasQueryFilter(u => !u.IsDeleted);

            // define relation
            builder.HasMany(e => e.IpWhiteLists)
                .WithOne(e => e.User)
                .HasForeignKey(e=>e.Id);

        }
    }
}
