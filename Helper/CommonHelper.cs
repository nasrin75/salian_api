
namespace salian_api.Helper
{
    public static class CommonHelper
    {
        public static string GenerateToken()
        {
            var random = new Random();
            var token = random.Next(99999, 999999);
            return token.ToString();
        }
    }
}
