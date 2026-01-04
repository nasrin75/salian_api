using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace salian_api.Entities
{
    [Table("IpWhiteLists")]
    public class IpWhiteListEntity
    {
        public long Id { get; set; }
        public IPAddress Ip { get; set; }
        public long UserID { get; set; }
        public UserEntity User { get; set; }
    }
}
