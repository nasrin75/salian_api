namespace salian_api.Dtos.Auth
{
    public class VerifyResetPasswordDto
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
        public required string Token { get; set; }
    }
}
