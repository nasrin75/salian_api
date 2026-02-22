using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;
using salian_api.Entities;

namespace salian_api.Helper
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
