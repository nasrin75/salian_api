using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Features")]
    public class FeatureEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
