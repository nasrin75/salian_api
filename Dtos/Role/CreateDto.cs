using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.Role
{
    public class CreateDto
    {
        [Required]
        public required string FaName { get; set; }

        [Required]
        public required string EnName { get; set; }
    }
}
