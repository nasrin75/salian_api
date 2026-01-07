using salian_api.Entities;

namespace salian_api.Response
{
    public class EmployeeResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long LocationID { get; set; }
        public string? Location {get; set;}
    }
}
