namespace salian_api.Models
{
    public class InventoryFeature
    {
        public long Id { get; set; }
        public long InventoryId { get; set; }
        public long FeatureId { get; set; }
        public string Value { get; set; }
    }
}
