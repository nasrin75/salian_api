namespace salian_api.Models
{
    public class EquipmentFeature
    {
        public long Id { get; set; }
        public long FeatureId { get; set; }
        public long EquipmentId { get; set; }
        public bool IsShow { get; set; }
    }
}
