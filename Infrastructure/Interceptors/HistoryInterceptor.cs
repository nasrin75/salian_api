using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using salian_api.Entities;
using System.Security.Claims;
namespace salian_api.Infrastructure.Interceptors
{
	public class HistoryInterceptor(IHttpContextAccessor _httpContextAccessor) : SaveChangesInterceptor
	{


		public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
			DbContextEventData eventData,
			InterceptionResult<int> result,
			CancellationToken cancellationToken = default)
		{
			var context = eventData.Context;

			if (context == null)
				return await base.SavingChangesAsync(eventData, result, cancellationToken);

			var histories = new List<HistoryEntity>();

			foreach (var entry in context.ChangeTracker.Entries())
			{
				if (entry.Entity is HistoryEntity ||
					entry.State == EntityState.Detached ||
					entry.State == EntityState.Unchanged)
					continue;

				var changedColumns = new List<string>();
				var oldValues = new Dictionary<string, object>();
				var newValues = new Dictionary<string, object>();

				foreach (var property in entry.Properties)
				{
					if (entry.State == EntityState.Modified && property.IsModified)
					{
						changedColumns.Add(property.Metadata.Name);
						oldValues[property.Metadata.Name] = property.OriginalValue;
						newValues[property.Metadata.Name] = property.CurrentValue;
					}

					if (entry.State == EntityState.Added)
						newValues[property.Metadata.Name] = property.CurrentValue;

					if (entry.State == EntityState.Deleted)
						oldValues[property.Metadata.Name] = property.OriginalValue;
				}

				var userId = _httpContextAccessor.HttpContext?.User?
					.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				var audit = new HistoryEntity
				{
					UserId = string.IsNullOrEmpty(userId) ? null : long.Parse(userId),
					EmployeeId = string.IsNullOrEmpty(EmployeeId) ? null : long.Parse(EmployeeId),
					TableName = entry.Metadata.GetTableName(),
					ActionType = entry.State switch
					{
						EntityState.Added => ActionType.Create,
						EntityState.Modified => ActionType.Update,
						EntityState.Deleted => ActionType.Delete,
						_ => ActionType.Update
					},
					OldValues = oldValues.Any() ? JsonSerializer.Serialize(oldValues) : null,
					NewValues = newValues.Any() ? JsonSerializer.Serialize(newValues) : null,
					IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
					CreatedAt = DateTime.UtcNow
				};

				histories.Add(audit);
			}

			context.Set<HistoryEntity>().AddRange(histories);

			return await base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		[AttributeUsage(AttributeTargets.Property)]
		public class AuditIgnoreAttribute : Attribute
		{
		}
	}
}

