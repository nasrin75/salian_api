using System.Net;

namespace salian_api.Models
{
    public class Log
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public long EmployeeID { get; set; }
        public int ActionTypeId { get; set; }
        public string ModelType { get; set; }
        public long ModelId { get; set; }
        public string LogNumber { get; set; }
        public IPAddress Ip { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
