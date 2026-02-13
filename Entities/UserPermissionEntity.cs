using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("UserPermissions")]
    public class UserPermissionEntity
    {
        public long UserId { get; set; }
        public long PermissionId { get; set; }
    }
}
