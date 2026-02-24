using System;

namespace salian_api.Config.Permissions
{
    public static class Permissions
    {
        public static string Permission = "Permissions";

        public static class User
        {
            public const string GetAll = "user.list";
            public const string Create = "user.create";
            public const string Edit = "user.edit";
            public const string Delete = "user.delete";
            public const string History = "user.history";
            public const string AddPermission = "user.add_permission";
        }

        public static class Employee
        {
            public const string GetAll = "employee.list";
            public const string Create = "employee.create";
            public const string Edit = "employee.edit";
            public const string Delete = "employee.delete";
            public const string History = "employee.history";
        }

        public static class Equipment
        {
            public const string GetAll = "equipment.list";
            public const string Create = "equipment.create";
            public const string Edit = "equipment.edit";
            public const string Delete = "equipment.delete";
        }

        public static class Location
        {
            public const string GetAll = "location.list";
            public const string Create = "location.create";
            public const string Edit = "location.edit";
            public const string Delete = "location.delete";
        }

        public static class Inventory
        {
            public const string GetAll = "inventory.list";
            public const string Create = "inventory.create";
            public const string Edit = "inventory.edit";
            public const string Delete = "inventory.delete";
            public const string History = "inventory.history";
        }

        public static class Role
        {
            public const string GetAll = "role.list";
            public const string Create = "role.create";
            public const string Edit = "role.edit";
            public const string Delete = "role.delete";
            public const string AddPermission = "role.add_permission";
        }

        public static class ActionType
        {
            public const string GetAll = "action_type.list";
            public const string Create = "action_type.create";
            public const string Edit = "action_type.edit";
            public const string Delete = "action_type.delete";
        }

        public static class Feature
        {
            public const string GetAll = "feature.list";
            public const string Create = "feature.create";
            public const string Edit = "feature.edit";
            public const string Delete = "feature.delete";
        }

        public static class History
        {
            public const string GetAll = "history.list";
            public const string GetDetails = "history.details";
            public const string Delete = "history.delete";
        }
    }
}
