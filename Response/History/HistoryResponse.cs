using salian_api.Entities;

namespace salian_api.Response.History
{
    public class HistoryResponse
    {
        public long Id { get; set; }

        public string? User { get; set; }

        public ActionTypeMap ActionType { get; set; }
        public long? EntityId { get; set; }
        public string Entity { get; set; }

        public string? NewData { get; set; } // JSON

        //public string LogNumber { get; set; }
        public string Ip { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
