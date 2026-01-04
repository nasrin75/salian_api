
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Equipments")]
    public class EquipmentEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; } // 0 => internal , 1=>external
    }
}
