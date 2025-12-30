using System.ComponentModel.DataAnnotations;

namespace salian_api.Contracts.Role
{
    public class CreateRequest
    {
        [Required]
        public string FaName { get; set; }
        public string? EnName { get; set; }
    }
}
