using System;
using salian_api.Infrastructure.Data;

namespace salian_api.Seeder
{
    public class SeederProvider
    {
        private readonly IEnumerable<ISeeder> _seeders;

        public SeederProvider(IEnumerable<ISeeder> seeders) => _seeders = seeders;

        public async Task SeedAllAsync(ApplicationDbContext _dbContext, IServiceProvider services)
        {
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync(_dbContext, services);
            }
        }
    }
}
