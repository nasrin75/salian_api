using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.Employee
{
    public class EmployeeUpdateDto
    {
        [Required]
        public required long Id { get; set; }
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public long? LocationId { get; set; }
    }
}
