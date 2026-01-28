
using salian_api.Entities;
using salian_api.Routes;

namespace salian_api.Response
{
    public class FeatureResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<EquipmentResponse> Equipments { get; set; }

    }
}
