namespace salian_api.Response.Auth
{
    public class LoginResponse
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string ExpireDate { get; set; }
    }
}
