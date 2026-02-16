using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.Permission
{
    public class PermissionUpdateDto
    {
        [Key]
        public required long Id { get; set; }
        public string Name { get; set; }
         public string? Title { get; set; }
        public string? Category { get; set; }
    }
}
