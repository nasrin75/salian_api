using salian_api.Entities;

namespace salian_api.Seeder
{
    public class RoleSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext _dbContext, IServiceProvider services)
        {
            if (!_dbContext.Roles.Any())
            {
                var roles = new RoleEntity[]
                {
                    new RoleEntity() { EnName = "Admin", FaName = "ادمین" },
                    new RoleEntity() { EnName = "User", FaName = "کاربر" },
                };

                _dbContext.Roles.AddRange(roles);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
