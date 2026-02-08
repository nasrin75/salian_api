

using salian_api.Entities;

namespace salian_api.Dtos.Equipment
{
    public class EquipmentCreateDto
    {
        public string Name { get; set; }
        public TypeMap Type { get; set; }
        public bool IsShowInMenu { get; set; }
    }
}
