using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("EquipmentFeatures")]
    public class EquipmentFeatureEntity
    {
        public long Id { get; set; }
        public long FeatureId { get; set; }
        public long EquipmentId { get; set; }
    }
}
