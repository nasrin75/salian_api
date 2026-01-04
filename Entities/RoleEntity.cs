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

        public bool IsDeleted { get; set; }
    }
}
