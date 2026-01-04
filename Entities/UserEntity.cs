using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Users")]
    public class UserEntity
    {

        public long Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }
        /*public bool IsAdmin { get; set; } // TODO:: save in another table*/
        public bool IsCheckIp { get; set; }
        public int LoginType { get; set; } // 2 => otp , 1 => password

        public List<IpWhiteListEntity> IpWhiteLists { get; set; }

    }

}
