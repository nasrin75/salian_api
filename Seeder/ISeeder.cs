using salian_api.Entities;

namespace salian_api.Seeder
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext _dbContext,IServiceProvider services);
    }
}
