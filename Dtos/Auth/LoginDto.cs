namespace salian_api.Dtos.Auth
{
    public class LoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool IsOtp { get; set; }
    }
}
