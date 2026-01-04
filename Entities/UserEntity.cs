using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Users")]
    public class UserEntity
    {
        [Key]
        public long Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }
        /*public bool IsAdmin { get; set; } // TODO:: save in another table*/
        public bool IsCheckIp { get; set; }
        public bool IsDeleted { get; set; }

        public LoginTypes LoginType { get; set; }
        public StatusLists Status { get; set; }

        public List<IpWhiteListEntity>? IpWhiteLists { get; set; }

        public long RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }

    public enum LoginTypes {
        [Display(Name = "کلمه عبور")]
        password = 1,

        [Display(Name = "رمز یکبار مصرف")]
        otp = 2,
    };

    public enum StatusLists
    {
        [Display(Name = "فعال")]
        deactive = 0,

        [Display(Name = "غیرفعال")]
        active = 1,
    };

}
