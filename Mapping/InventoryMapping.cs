using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using salian_api.Entities;

namespace salian_api.Mapping
{
    public  class InventoryMapping :IEntityTypeConfiguration<InventoryEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryEntity> builder)
        {
            builder.ToTable("Inventories");
            builder.HasKey(x => x.Id);

            builder.Property("UserId")
                    .IsRequired();

            builder.Property("EmployeeId")
                    .IsRequired();

            builder.Property("EquipmentId")
                    .IsRequired();

            builder.Property("LocationID")
                    .IsRequired();

            builder.Property("ItNumber")
                    .IsRequired();

            builder.Property("ItParentNumber")
                    .IsRequired();

            builder.Property("Status")
                    .IsRequired();

            builder.Property("PropertyNumber")
                    .HasMaxLength(255)
                    .IsRequired(false);

            builder.Property("SerialNumber")
                   .HasMaxLength(255)
                   .IsRequired(false);


            builder.Property("InvoiceNumber")
                    .HasMaxLength(255)
                    .IsRequired(false);


            builder.Property("InvoiceImage")
                    .IsRequired(false);


            builder.Property("Description")
                    .HasMaxLength(255)
                    .IsRequired(false);

            builder.Property("BrandName")
                    .HasMaxLength(255)
                    .IsRequired(false);

            builder.Property("ModelName")
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property("Capacity")
                    .HasMaxLength(255)
                    .IsRequired(false);

            builder.Property("Size")
                    .IsRequired(false);

            builder.Property("ExpireWarrantyDate")
                    .IsRequired(false);

            builder.Property("DeliveryDate")
                   .IsRequired(false);

            builder.Property("CreatedAt")
                    .IsRequired();

            builder.Property("UpdatedAt")
                    .IsRequired();

            builder.Property("DeletedAt")
                    .IsRequired(false);

        }
    }
}
