
namespace salian_api.Models
{
    public class Equipment
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; } // 0 => internal , 1=>external
    }
}
