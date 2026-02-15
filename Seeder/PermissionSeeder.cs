using System;
using salian_api.Config.Permissions;
using salian_api.Entities;
using salian_api.Routes;

namespace salian_api.Seeder
{
    public class PermissionSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext _dbContext, IServiceProvider services)
        {
            if (!_dbContext.Permissions.Any())
            {
                var permissions = new PermissionEntity[]
                {
                    //Inventory
                    new PermissionEntity() { Name = Permissions.Inventory.GetAll },
                    new PermissionEntity() { Name = Permissions.Inventory.Create },
                    new PermissionEntity() { Name = Permissions.Inventory.Edit },
                    new PermissionEntity() { Name = Permissions.Inventory.Delete },
                    new PermissionEntity() { Name = Permissions.Inventory.History },
                    //equipment
                    new PermissionEntity() { Name = Permissions.Equipment.GetAll },
                    new PermissionEntity() { Name = Permissions.Equipment.Create },
                    new PermissionEntity() { Name = Permissions.Equipment.Edit },
                    new PermissionEntity() { Name = Permissions.Equipment.Delete },
                    //User
                    new PermissionEntity() { Name = Permissions.User.GetAll },
                    new PermissionEntity() { Name = Permissions.User.Create },
                    new PermissionEntity() { Name = Permissions.User.Edit },
                    new PermissionEntity() { Name = Permissions.User.Delete },
                    new PermissionEntity() { Name = Permissions.User.AddPermission },
                    //Employee
                    new PermissionEntity() { Name = Permissions.Employee.GetAll },
                    new PermissionEntity() { Name = Permissions.Employee.Create },
                    new PermissionEntity() { Name = Permissions.Employee.Edit },
                    new PermissionEntity() { Name = Permissions.Employee.Delete },
                    //Location
                    new PermissionEntity() { Name = Permissions.Location.GetAll },
                    new PermissionEntity() { Name = Permissions.Location.Create },
                    new PermissionEntity() { Name = Permissions.Location.Edit },
                    new PermissionEntity() { Name = Permissions.Location.Delete },
                    //ActionType
                    new PermissionEntity() { Name = Permissions.ActionType.GetAll },
                    new PermissionEntity() { Name = Permissions.ActionType.Create },
                    new PermissionEntity() { Name = Permissions.ActionType.Edit },
                    new PermissionEntity() { Name = Permissions.ActionType.Delete },
                    //Role
                    new PermissionEntity() { Name = Permissions.Role.GetAll },
                    new PermissionEntity() { Name = Permissions.Role.Create },
                    new PermissionEntity() { Name = Permissions.Role.Edit },
                    new PermissionEntity() { Name = Permissions.Role.Delete },
                    new PermissionEntity() { Name = Permissions.Role.AddPermission },
                    //Feature
                    new PermissionEntity() { Name = Permissions.Feature.GetAll },
                    new PermissionEntity() { Name = Permissions.Feature.Create },
                    new PermissionEntity() { Name = Permissions.Feature.Edit },
                    new PermissionEntity() { Name = Permissions.Feature.Delete },
                };

                _dbContext.Permissions.AddRange(permissions);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
