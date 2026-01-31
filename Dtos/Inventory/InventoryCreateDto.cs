using salian_api.Entities;
using System.Runtime.CompilerServices;

namespace salian_api.Dtos.Inventory
{
    public class InventoryCreateDto
    {
        public long ItNumber { get; set; }
        public long ItParentNumber { get; set; }
        public long UserId { get; set; }
        public long EmployeeId { get; set; }
        public long LocationId { get; set; }
        public long EquipmentId { get; set; }
        public StatusMap Status { get; set; }
        public string PropertyNumber { get; set; }
        public string SerialNumber { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? InvoiceImage { get; set; }
        public string? Description { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string? Capacity { get; set; }
        public string? Size { get; set; }
        
        public DateOnly? ExpireWarrantyDate { get; set; }
        public DateOnly? DeliveryDate { get; set; }
    }
}
