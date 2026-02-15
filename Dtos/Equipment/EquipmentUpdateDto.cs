using System.ComponentModel.DataAnnotations;
using salian_api.Entities;

namespace salian_api.Dtos.Equipment
{
    public class EquipmentUpdateDto
    {
        [Key]
        [Required]
        public required long Id { get; set; }
        public string? Name { get; set; }
        public TypeMap Type { get; set; }
        public bool IsShowInMenu { get; set; }
    }
}
