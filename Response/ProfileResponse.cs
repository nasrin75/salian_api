namespace salian_api.Response
{
    public class ProfileResponse
    {
        public long Id { get; set; }

        public string Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
    }
}
