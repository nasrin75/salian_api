using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.Feature
{
    public class FeatureUpdateDto
    {
        [Key]
        public required long Id { get; set; }
        public string? Name { get; set; }
        public List<long> EquipmentIds { get; set; }
    }
}
