using salian_api.Entities;

namespace salian_api.Dtos.Inventory
{
    public class SearchInventoryDto
    {
        public long ItNumber { get; set; }
        public long ItParentNumber { get; set; }
        public long UserId { get; set; }
        public long EmployeeId { get; set; }
        public long LocationID { get; set; }
        public long EquipmentId { get; set; }
        public StatusMap Status { get; set; }
        public string? PropertyNumber { get; set; }
        public string? SerialNumber { get; set; }
        public string? InvoiceNumber { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
