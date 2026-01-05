using salian_api.Entities;
using salian_api.Response;
using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.User
{
    public class UserResponse
    {
        public  long Id { get; set; }
        public  string Username { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsCheckIp { get; set; }

        public LoginTypes LoginType { get; set; }
        public StatusLists? Status { get; set; }

        public long RoleId { get; set; }

        public List<IpWhiteListResponse>? IpWhiteLists { get; set; }
    }
    public enum LoginTypes
    {
        [Display(Name = "کلمه عبور")]
        password = 1,

        [Display(Name = "رمز یکبار مصرف")]
        otp = 2,
    };


}
