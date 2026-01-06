using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.Role
{
    public class RoleUpdateDto
    {
        [Required]
        public required long Id { get; set; }
        public string? FaName { get; set; }
        public string? EnName { get; set; }
    }
}
