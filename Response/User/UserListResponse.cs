
using salian_api.Entities;

namespace salian_api.Response
{
    public class UserListResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string Mobile { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; }
    }
}
