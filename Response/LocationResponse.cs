using salian_api.Entities;

namespace salian_api.Response
{
    public class LocationResponse
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
        public bool IsShow { get; set; }
    }
}
