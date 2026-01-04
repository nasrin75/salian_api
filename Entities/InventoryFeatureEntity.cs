using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("InventoryFeatures")]
    public class InventoryFeatureEntity
    {
        public long Id { get; set; }
        public long InventoryId { get; set; }
        public long FeatureId { get; set; }
        public string Value { get; set; }
    }
}
