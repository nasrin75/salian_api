namespace salian_api.Dtos.Profile
{
    public class ProfileUpdateDto
    {
        public required long Id { get; set; }

        public string? Username { get; set; }
        public string? NewPassword { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
    }
}
