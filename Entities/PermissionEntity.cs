using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Permissions")]
    public class PermissionEntity
    {
        public long Id { get; set; }
        public required string Name { get; set; }
    }
}
