using System.Net;

namespace salian_api.Models
{
    public class IpWhiteList
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public IPAddress Ip { get; set; }
    }
}
