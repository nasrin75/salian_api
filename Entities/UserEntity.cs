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
        public bool IsCheckIp { get; set; }
        public bool IsDeleted { get; set; }

        public List<LoginTypes> LoginTypes { get; set; } = new List<LoginTypes>();
        public StatusLists Status { get; set; }

        public ICollection<IpWhiteListEntity>? IpWhiteLists { get; set; }

        public long RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }

    public class LoginTypeJson
    {
        public string Name { get; set; }
    }
    public enum LoginTypes {
        password,
        otp ,
        push,
        email
    };

    public enum StatusLists
    {
        [Display(Name = "فعال")]
        deactive = 0,

        [Display(Name = "غیرفعال")]
        active = 1,
    };

}
