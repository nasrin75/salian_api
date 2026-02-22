using salian_api.Entities;
using salian_api.Helper;
using salian_api.Infrastructure.Data;

namespace salian_api.Seeder
{
    public class UserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext _dbContext, IServiceProvider services)
        {
            if (!_dbContext.Users.Any())
            {
                var user = new UserEntity
                {
                    Username = "nasrin",
                    Password = PasswordHelper.HashPassword("Nasrin@1234"),
                    Status = (StatusLists)1,
                    RoleId = 1,
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
