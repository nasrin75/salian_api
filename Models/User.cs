using System.ComponentModel.DataAnnotations;

namespace salian_api.Models
{
    public class User
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "مقدار این فیلد الزامی است.")]
        [MinLength(3, ErrorMessage = "حداقل طول این فیلد ۳ کاراکتر هست.")]
        [MaxLength(100, ErrorMessage = "حداکثر طول این فیلد ۳ کاراکتر هست.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "مقدار این فیلد الزامی است.")]
        public string Password { get; set; }

        [EmailAddress()]
        public string? Email { get; set; }

        public string? Mobile { get; set; }
        public bool IsAdmin { get; set; } // TODO:: save in another table
        public bool IsCheckIP { get; set; }
        public bool LoginType { get; set; } // 2 => otp , 1 => password

    }

}
