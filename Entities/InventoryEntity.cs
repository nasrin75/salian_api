using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Inventories")]
    public class InventoryEntity
    {
        [Key]
        public long Id { get; set; }
        public long ItNumber { get; set; }
        public long ItParentNumber { get; set; }

        public long UserId { get; set; }
        public UserEntity User { get; set; }

        public long EmployeeId { get; set; }
        public EmployeeEntity Employee { get; set; }


        public long LocationID { get; set; }
        public LocationEntity Location { get; set; }

        public EquipmentEntity Equipment { get; set; }
        public long EquipmentId { get; set; }

        public StatusMap Status { get; set; }
        public string? PropertyNumber { get; set; }
        public string? SerialNumber { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? InvoiceImage { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string? Capacity { get; set; }
        public string? Size { get; set; }
        public DateOnly? ExpireWarrantyDate { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        // feature -> Clock , Core, Bus, RamType

        public InventoryEntity()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public enum StatusMap
    {
        Useless = -1, // اسقاطی
        Unuse = 0, // استفاده نشده
        Inuse = 1,// استفاده شده
        SendToCharge = 2, // ارسال جهت شارژ
        BackFromCharge = 3, // بازگشت از شارژ
    }

    /*public  string function GetStatusWithID(int id)
    {
        var status = "Unuse";
        switch (id)
        {
            case -1:
                status = "Useless";
                break;
            case 0:
                status = "Unuse";
                break;
            case 1:
                status = "Inuse";
                break;
            case 2:
                status = "SendToCharge";
                break;
            case 3:
                status = "BackFromCharge";
                break;
        }

        return status;
    }*/
}
