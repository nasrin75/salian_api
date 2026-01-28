using salian_api.Entities;

namespace salian_api.Response.Inventory
{
    public class InventoryListResponse
    {
        public long Id { get; set; }
        public long ItNumber { get; set; }
        public long ItParentNumber { get; set; }
        public string User { get; set; }
        public string? Employee { get; set; }
        public string? Location { get; set; }
        public string? Equipment { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
