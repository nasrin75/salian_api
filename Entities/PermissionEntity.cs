using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Permissions")]
    public class PermissionEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<RoleEntity?> Roles { get; set; }
        public List<UserEntity?> Users { get; set; }
    }
}
