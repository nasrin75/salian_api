using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using salian_api.Entities;

public class HistoryInterceptor : SaveChangesInterceptor
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	// نگه داشتن Added ها تا بعد از Save
	private readonly List<EntityEntry> _addedEntries = new();

	public HistoryInterceptor(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	// -------------------------
	// BEFORE SAVE (Update/Delete)
	// -------------------------
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
			if (entry.Entity is HistoryEntity)
				continue;

			if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
				continue;

			// Added رو فعلاً ذخیره کن برای بعد از Save
			if (entry.State == EntityState.Added)
			{
				_addedEntries.Add(entry);
				continue;
			}

			var oldValues = new Dictionary<string, object>();
			var newValues = new Dictionary<string, object>();

			bool isSoftDelete = false;
			bool isRestore = false;

			// بررسی SoftDelete با DeletedAt
			var deletedAtProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "DeletedAt");
			if (deletedAtProp != null && deletedAtProp.IsModified)
			{
				var original = deletedAtProp.OriginalValue as DateTime?;
				var current = deletedAtProp.CurrentValue as DateTime?;

				if (original == null && current != null) isSoftDelete = true;
				if (original != null && current == null) isRestore = true;
			}

			foreach (var property in entry.Properties)
			{
				if (property.Metadata.IsPrimaryKey())
					continue;

				// اگر Attribute داشت Ignore کن
				if (property.Metadata.PropertyInfo?
					.GetCustomAttributes(typeof(AuditIgnoreAttribute), false).Any() == true)
					continue;

				// DELETE یا SoftDelete
				if (entry.State == EntityState.Deleted || isSoftDelete)
				{
					oldValues[property.Metadata.Name] = property.OriginalValue;
				}
				// UPDATE
				else if (entry.State == EntityState.Modified && property.IsModified)
				{
					var original = property.OriginalValue?.ToString();
					var current = property.CurrentValue?.ToString();

					if (original == current)
						continue;

					oldValues[property.Metadata.Name] = original;
					newValues[property.Metadata.Name] = current;
				}

				// بررسی ForeignKey برای فیلد نمایشی
				await AddForeignKeyDisplayValuesAsync(context, entry, property, newValues, cancellationToken);
			}

			if (!oldValues.Any() && !isSoftDelete && entry.State == EntityState.Modified)
				continue;

			histories.Add(CreateHistory(entry, oldValues, newValues, false, isSoftDelete, isRestore));
		}

		if (histories.Any())
			context.Set<HistoryEntity>().AddRange(histories);

		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	// -------------------------
	// AFTER SAVE (Create)
	// -------------------------
	public override async ValueTask<int> SavedChangesAsync(
		SaveChangesCompletedEventData eventData,
		int result,
		CancellationToken cancellationToken = default)
	{
		var context = eventData.Context;
		if (context == null || !_addedEntries.Any())
			return await base.SavedChangesAsync(eventData, result, cancellationToken);

		var histories = new List<HistoryEntity>();

		foreach (var entry in _addedEntries)
		{
			var newValues = new Dictionary<string, object>();

			foreach (var property in entry.Properties)
			{
				if (property.Metadata.IsPrimaryKey())
					continue;

				if (property.Metadata.PropertyInfo?
					.GetCustomAttributes(typeof(AuditIgnoreAttribute), false).Any() == true)
					continue;

				newValues[property.Metadata.Name] = property.CurrentValue;

				await AddForeignKeyDisplayValuesAsync(context, entry, property, newValues, cancellationToken);
			}

			histories.Add(CreateHistory(entry, null, newValues, true, false, false));
		}

		_addedEntries.Clear();

		if (histories.Any())
		{
			context.Set<HistoryEntity>().AddRange(histories);
			await context.SaveChangesAsync(cancellationToken);
		}

		return await base.SavedChangesAsync(eventData, result, cancellationToken);
	}

	// -------------------------
	// Helper
	// -------------------------
	private HistoryEntity CreateHistory(
		EntityEntry entry,
		Dictionary<string, object>? oldValues,
		Dictionary<string, object>? newValues,
		bool isCreate = false,
		bool isSoftDelete = false,
		bool isRestore = false)
	{
		var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());

		long recordId = 0;
		if (primaryKey?.CurrentValue != null)
			long.TryParse(primaryKey.CurrentValue.ToString(), out recordId);

		var userIdStr = _httpContextAccessor
			.HttpContext?.User?
			.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		long? userId = null;
		if (!string.IsNullOrEmpty(userIdStr))
			userId = long.Parse(userIdStr);

		var ip = _httpContextAccessor.HttpContext?
			.Connection.RemoteIpAddress?.ToString();

		return new HistoryEntity
		{
			UserId = userId,
			TableName = entry.Metadata.GetTableName(),
			RecordId = recordId,
			ActionType = isCreate
				? ActionTypeMap.CREATE
				: isSoftDelete
					? ActionTypeMap.DELETE
					: isRestore
						? ActionTypeMap.DELETE
						: ActionTypeMap.UPDATE,
			OldValues = oldValues != null && oldValues.Any()
				? JsonSerializer.Serialize(oldValues)
				: null,
			NewValues = newValues != null && newValues.Any()
				? JsonSerializer.Serialize(newValues)
				: null,
			IpAddress = ip
		};
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class AuditIgnoreAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Property)]
	public class DisplayFieldAttribute : Attribute { }

	private async Task AddForeignKeyDisplayValuesAsync(
		DbContext context,
		EntityEntry entry,
		PropertyEntry property,
		Dictionary<string, object> newValues,
		CancellationToken cancellationToken)
	{
		var foreignKey = entry.Metadata
			.GetForeignKeys()
			.FirstOrDefault(fk => fk.Properties.Any(p => p.Name == property.Metadata.Name));

		if (foreignKey == null)
			return;

		var principalType = foreignKey.PrincipalEntityType.ClrType;
		var fkValue = property.CurrentValue;

		if (fkValue == null)
			return;

		var relatedEntity = await context.FindAsync(principalType, fkValue, cancellationToken);
		if (relatedEntity == null)
			return;

		var displayProp = principalType
			.GetProperties()
			.FirstOrDefault(p => p.GetCustomAttributes(typeof(DisplayFieldAttribute), false).Any())
			?? principalType.GetProperty("Name")
			?? principalType.GetProperty("Title")
			?? principalType.GetProperties().FirstOrDefault(p => p.PropertyType == typeof(string));

		if (displayProp == null)
			return;

		var displayValue = displayProp.GetValue(relatedEntity);
		var navigationName = foreignKey.DependentToPrincipal?.Name ?? principalType.Name;

		newValues[$"{navigationName}{displayProp.Name}"] = displayValue;
	}
}