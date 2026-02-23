using salian_api.Infrastructure.Data;

namespace salian_api.Seeder
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext _dbContext, IServiceProvider services);
    }
}
