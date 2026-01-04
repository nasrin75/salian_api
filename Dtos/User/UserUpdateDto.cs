using salian_api.Entities;
using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.User
{
    public class UserUpdateDto
    {
        [Required]
        public required long Id { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public bool? IsCheckIp { get; set; }
        public long? RoleId { get; set; }
        public LoginTypes? LoginType { get; set; }
        public StatusLists? Status { get; set; }
    }
}
