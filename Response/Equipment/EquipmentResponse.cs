using salian_api.Entities;

namespace salian_api.Response.Equipment
{
    public class EquipmentResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TypeMap Type { get; set; }
        public bool IsShowInMenu { get; set; }
    }
}
