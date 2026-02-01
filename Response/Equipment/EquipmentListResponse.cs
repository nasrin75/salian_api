using salian_api.Entities;

namespace salian_api.Response.Equipment
{
    public class EquipmentListResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TypeMap Type { get; set; }
    }
}
