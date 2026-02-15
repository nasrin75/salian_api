using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Roles")]
    public class RoleEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string FaName { get; set; }

        [Required]
        public string EnName { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<UserEntity> Users { get; set; }

        //Many to Many relation
        public List<PermissionEntity> Permissions { get; set; } = [];
    }
}
