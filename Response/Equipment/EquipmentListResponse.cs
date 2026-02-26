using salian_api.Entities;

namespace salian_api.Response.Equipment
{
    public class EquipmentListResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TypeMap Type { get; set; }
        public int? UnsedCount { get; set; } = 0;
        public int? UsedCount { get; set; } = 0;
        public int? SendToChargeCount { get; set; } = 0;
        public int? BackFromChargeCount { get; set; } = 0;
        public int? RepairCount { get; set; } = 0;
        public int? UselessCount { get; set; } = 0;
    }
}
