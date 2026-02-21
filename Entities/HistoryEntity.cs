using System;
namespace salian_api.Entities
{
	public class HistoryEntity
	{
		public long Id { get; set; }

		public long? UserId { get; set; }
		public long? EmployeeId { get; set; }

		public ActionType ActionType { get; set; }

		public string TableName { get; set; }
		public long? RecordId { get; set; }

		public string? OldValues { get; set; }   // JSON
		public string? NewValues { get; set; }   // JSON

		//public string LogNumber { get; set; }
		public string IpAddress { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
	public enum ActionType
	{
		Create = 1,
		Update = 2,
		Delete = 3,
		Login = 4
	}
}

