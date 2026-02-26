using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Equipments")]
    public class EquipmentEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public TypeMap Type { get; set; }
        public bool IsShowInMenu { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public List<FeatureEntity> Features { get; set; }
        public List<InventoryEntity> Inventories { get; set; }
    }

    public enum TypeMap
    {
        Internal = 1,
        External = 2,
    }
}
