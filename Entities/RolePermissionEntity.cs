using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("RolePermissions")]
    public class RolePermissionEntity
    {
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
    }
}
