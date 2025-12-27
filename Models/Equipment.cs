using System.ComponentModel.DataAnnotations;

namespace salian_api.Models
{
    public class Equipment
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "مقدار این فیلد الزامی است.")]
        public string Name { get; set; }
        public int Type { get; set; } // 0 => internal , 1=>external
    }
}
