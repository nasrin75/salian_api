using System;
using salian_api.Config.Permissions;
using salian_api.Entities;
using salian_api.Infrastructure.Data;
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
                    new PermissionEntity()
                    {
                        Name = Permissions.Inventory.GetAll,
                        Title = "لیست انبار",
                        Category = "انبار",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Inventory.Create,
                        Title = "افزودن قطعه به انبار",
                        Category = "انبار",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Inventory.Edit,
                        Title = "ویرایش انبار",
                        Category = "انبار",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Inventory.Delete,
                        Title = "حذف از انبار",
                        Category = "انبار",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Inventory.History,
                        Title = "تاریخچه انبار",
                        Category = "انبار",
                    },
                    //equipment
                    new PermissionEntity()
                    {
                        Name = Permissions.Equipment.GetAll,
                        Title = "لیست قطعات",
                        Category = "قطعات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Equipment.Create,
                        Title = "ساخت قطعه",
                        Category = "قطعات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Equipment.Edit,
                        Title = "ویرایش قطعه",
                        Category = "قطعات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Equipment.Delete,
                        Title = "حذف قطعه",
                        Category = "قطعات",
                    },
                    //User
                    new PermissionEntity()
                    {
                        Name = Permissions.User.GetAll,
                        Title = "لیست کاربران",
                        Category = "کاربران",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.User.Create,
                        Title = "ایجاد کاربر",
                        Category = "کاربران",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.User.Edit,
                        Title = "ویرایش کاربر",
                        Category = "کاربران",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.User.Delete,
                        Title = "حذف کاربر",
                        Category = "کاربران",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.User.History,
                        Title = "تاریخچه کاربر",
                        Category = "کاربران",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.User.AddPermission,
                        Title = "افزودن دسترسی به کاربران",
                        Category = "کاربران",
                    },
                    //Employee
                    new PermissionEntity()
                    {
                        Name = Permissions.Employee.GetAll,
                        Title = "لیست پرسنل",
                        Category = "پرسنل",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Employee.Create,
                        Title = "ایجاد پرسنل جدید",
                        Category = "پرسنل",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Employee.Edit,
                        Title = "ویرایش پرسنل",
                        Category = "پرسنل",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Employee.Delete,
                        Title = "حذف پرسنل",
                        Category = "پرسنل",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Employee.History,
                        Title = "تاریخچه پرسنل",
                        Category = "پرسنل",
                    },
                    //Location
                    new PermissionEntity()
                    {
                        Name = Permissions.Location.GetAll,
                        Title = "لیست بخش ها",
                        Category = "بخش ها",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Location.Create,
                        Title = "ایجاد بخش",
                        Category = "بخش ها",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Location.Edit,
                        Title = "ویرایش بخش",
                        Category = "بخش ها",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Location.Delete,
                        Title = "حذف بخش",
                        Category = "بخش ها",
                    },
                    //ActionType
                    new PermissionEntity()
                    {
                        Name = Permissions.ActionType.GetAll,
                        Title = "لیست انواع عملیات",
                        Category = "انواع عملیات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.ActionType.Create,
                        Title = "ایجاد انواع عملیات",
                        Category = "انواع عملیات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.ActionType.Edit,
                        Title = "ویرایش انواع عملیات",
                        Category = "انواع عملیات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.ActionType.Delete,
                        Title = "حذف انواع عملیات",
                        Category = "انواع عملیات",
                    },
                    //Role
                    new PermissionEntity()
                    {
                        Name = Permissions.Role.GetAll,
                        Title = "لیست نقش ها",
                        Category = "نقش ها",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Role.Create,
                        Title = "ایجاد نقش",
                        Category = "نقش ها",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Role.Edit,
                        Title = "ویرایش نقش",
                        Category = "نقش ها",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Role.Delete,
                        Title = "حذف نقش",
                        Category = "نقش ها",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Role.AddPermission,
                        Title = "افزودن دسترسی به نقش ها",
                        Category = "نقش ها",
                    },
                    //Feature
                    new PermissionEntity()
                    {
                        Name = Permissions.Feature.GetAll,
                        Title = "لیست ویژگی قطعات",
                        Category = "ویژگی قطعات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Feature.Create,
                        Title = "ایجاد ویژگی قطعات",
                        Category = "ویژگی قطعات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Feature.Edit,
                        Title = "ویرایش ویژگی قطعات",
                        Category = "ویژگی قطعات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.Feature.Delete,
                        Title = "حذف ویژگی قطعات",
                        Category = "ویژگی قطعات",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.History.GetAll,
                        Title = "لیست تاریخچه ها",
                        Category = "تاریخچه",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.History.GetDetails,
                        Title = "جزییات تاریخچه",
                        Category = "تاریخچه",
                    },
                    new PermissionEntity()
                    {
                        Name = Permissions.History.Delete,
                        Title = "حذف تاریخچه",
                        Category = "تاریخچه",
                    },
                };

                _dbContext.Permissions.AddRange(permissions);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
