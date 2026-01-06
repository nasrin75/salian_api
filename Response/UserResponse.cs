using salian_api.Entities;

namespace salian_api.Response
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsCheckIp { get; set; }

        public List<string> LoginTypes { get; set; }
        public StatusLists? Status { get; set; }

        public long RoleId { get; set; }

        public List<IpWhiteListResponse>? IpWhiteLists { get; set; }
    }
}
