using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Inventories")]
    public class InventoryEntity
    {
        public long Id { get; set; }
        public long ItNumber { get; set; }
        public long ItParentNumber { get; set; }
        public long UserId { get; set; }
        public long EmployeeId { get; set; }
        public int LocationID { get; set; }
        public int EquipmentId { get; set; }
        public int Status { get; set; }
        public string PropertyNumber { get; set; }
        public string SerialNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceImage { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string RamType { get; set; }
        public int Capacity { get; set; }
        public int Size { get; set; }
        public int Bus { get; set; }
        // clock
        public string Clock { get; set; }
        public string Core { get; set; }
        public DateOnly ExpireWarrantyDate { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
