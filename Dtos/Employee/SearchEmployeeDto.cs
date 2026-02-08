using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.Employee
{
    public class SearchEmployeeDto
    {
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public long? LocationId { get; set; }
    }
}
