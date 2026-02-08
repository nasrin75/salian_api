using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("IpWhiteLists")]
    public class IpWhiteListEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Ip { get; set; }
        public string IpRange { get; set; }
        public long UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
