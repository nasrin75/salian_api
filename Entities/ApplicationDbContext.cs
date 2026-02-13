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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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

        // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        // {
        //     var history = new List<LogEntity>();

        //     var user = _httpContextAccessor.HttpContext?.User;

        //     var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //     var userName = user?.Identity?.Name;
        //     var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        //     foreach (var entry in ChangeTracker.Entries())
        //     {
        //         if (entry.Entity is AuditLogEntity ||
        //             entry.State == EntityState.Detached ||
        //             entry.State == EntityState.Unchanged)
        //             continue;

        //         var audit = new AuditLogEntity
        //         {
        //             UserId = userId,
        //             UserName = userName,
        //             TableName = entry.Metadata.GetTableName()!,
        //             Action = entry.State.ToString(),
        //             DateTime = DateTime.UtcNow,
        //             IpAddress = ip
        //         };

        //         var oldValues = new Dictionary<string, object?>();
        //         var newValues = new Dictionary<string, object?>();

        //         foreach (var property in entry.Properties)
        //         {
        //             string propName = property.Metadata.Name;

        //             if (entry.State == EntityState.Added)
        //             {
        //                 newValues[propName] = property.CurrentValue;
        //             }
        //             else if (entry.State == EntityState.Deleted)
        //             {
        //                 oldValues[propName] = property.OriginalValue;
        //             }
        //             else if (entry.State == EntityState.Modified && property.IsModified)
        //             {
        //                 oldValues[propName] = property.OriginalValue;
        //                 newValues[propName] = property.CurrentValue;
        //             }
        //         }

        //         audit.OldValues = JsonSerializer.Serialize(oldValues);
        //         audit.NewValues = JsonSerializer.Serialize(newValues);

        //         history.Add(audit);
        //     }

        //     var result = await base.SaveChangesAsync(cancellationToken);

        //     if (history.Count > 0)
        //     {
        //         LogEntity.AddRange(history);
        //         await base.SaveChangesAsync(cancellationToken);
        //     }

        //     return result;
        //     //Add in program.cs
        //     //builder.Services.AddHttpContextAccessor();
        // }
    }
}
