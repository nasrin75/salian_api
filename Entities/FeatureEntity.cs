using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Features")]
    public class FeatureEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
