using salian_api.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace salian_api.Dtos.User
{
    public class UserCreateDto
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public bool? IsCheckIp { get; set; }
        public long RoleId { get; set; }
        public LoginTypes? LoginType { get; set; }
        public StatusLists? Status { get; set; }

        public List<string>? IpWhiteLists { get; set; }
    }
}
