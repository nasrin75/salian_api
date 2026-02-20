using System.Security.Claims;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using salian_api.Mapping;

namespace salian_api.Entities
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<EquipmentEntity> Equipments { get; set; }
        public DbSet<ActionTypeEntity> ActionTypes { get; set; }
        public DbSet<IpWhiteListEntity> IpWhiteLists { get; set; }
        public DbSet<InventoryEntity> Inventories { get; set; }
        public DbSet<InventoryFeatureEntity> InventoryFeatures { get; set; }
        public DbSet<FeatureEntity> Features { get; set; }
        public DbSet<HistoryEntity> Histories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor _httpContextAccessor) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleMapping());
            modelBuilder.ApplyConfiguration(new ActionTypeMapping());
            modelBuilder.ApplyConfiguration(new LocationMapping());
            modelBuilder.ApplyConfiguration(new EmployeeMapping());
            modelBuilder.ApplyConfiguration(new IpWhiteListMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new EquipmentMapping());
            modelBuilder.ApplyConfiguration(new FeatureMapping());
            modelBuilder.ApplyConfiguration(new InventoryMapping());
            modelBuilder.ApplyConfiguration(new InventoryFeatureMapping());
            modelBuilder.ApplyConfiguration(new PermissionMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
