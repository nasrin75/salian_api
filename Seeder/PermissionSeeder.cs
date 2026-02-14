using System;
using salian_api.Entities;
using salian_api.Routes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                    new PermissionEntity() { Name = "inventory.list" },
                    new PermissionEntity() { Name = "inventory.create" },
                    new PermissionEntity() { Name = "inventory.edit" },
                    new PermissionEntity() { Name = "inventory.delete" },
                    new PermissionEntity() { Name = "inventory.history" },
                    //equipment
                    new PermissionEntity() { Name = "equipment.list" },
                    new PermissionEntity() { Name = "equipment.create" },
                    new PermissionEntity() { Name = "equipment.edit" },
                    new PermissionEntity() { Name = "equipment.delete" },
                    //User
                    new PermissionEntity() { Name = "user.list" },
                    new PermissionEntity() { Name = "user.create" },
                    new PermissionEntity() { Name = "user.edit" },
                    new PermissionEntity() { Name = "user.delete" },
                    new PermissionEntity() { Name = "user.add_permission" },
                    //Employee
                    new PermissionEntity() { Name = "employee.list" },
                    new PermissionEntity() { Name = "employee.create" },
                    new PermissionEntity() { Name = "employee.edit" },
                    new PermissionEntity() { Name = "employee.delete" },
                    //Employee
                    new PermissionEntity() { Name = "employee.list" },
                    new PermissionEntity() { Name = "employee.create" },
                    new PermissionEntity() { Name = "employee.edit" },
                    new PermissionEntity() { Name = "employee.delete" },
                    //Location
                    new PermissionEntity() { Name = "location.list" },
                    new PermissionEntity() { Name = "location.create" },
                    new PermissionEntity() { Name = "location.edit" },
                    new PermissionEntity() { Name = "location.delete" },
                    //ActionType
                    new PermissionEntity() { Name = "action_type.list" },
                    new PermissionEntity() { Name = "action_type.create" },
                    new PermissionEntity() { Name = "action_type.edit" },
                    new PermissionEntity() { Name = "action_type.delete" },
                    //Role
                    new PermissionEntity() { Name = "role.list" },
                    new PermissionEntity() { Name = "role.create" },
                    new PermissionEntity() { Name = "role.edit" },
                    new PermissionEntity() { Name = "role.delete" },
                    new PermissionEntity() { Name = "role.add_permission" },
                };

                _dbContext.Permissions.AddRange(permissions);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
