using salian_api.Entities;

namespace salian_api.Dtos.Equipment
{
    public class SearchEquipmentDto
    {
        public string? Name { get; set; }
        public TypeMap? Type { get; set; }
    }
}
