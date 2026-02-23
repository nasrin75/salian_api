using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using salian_api.Entities;

public class HistoryInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HistoryInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        var context = eventData.Context;
        if (context == null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var histories = new List<HistoryEntity>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            // don't log history model
            if (entry.Entity is HistoryEntity)
                continue;

            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var oldValues = new Dictionary<string, object>();
            var newValues = new Dictionary<string, object>();

            //  primary key → RecordId
            var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());

            long recordId = 0;
            if (primaryKey?.CurrentValue != null)
                long.TryParse(primaryKey.CurrentValue.ToString(), out recordId);

            // ---------- CREATE ----------
            if (entry.State == EntityState.Added)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.IsPrimaryKey())
                        continue;

                    newValues[property.Metadata.Name] = property.CurrentValue;
                }
            }
            // ---------- DELETE ----------
            else if (entry.State == EntityState.Deleted)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.IsPrimaryKey())
                        continue;

                    oldValues[property.Metadata.Name] = property.OriginalValue;
                }
            }
            // ---------- UPDATE (just changes filed) ----------
            else if (entry.State == EntityState.Modified)
            {
                foreach (var property in entry.Properties)
                {
                    if (!property.IsModified)
                        continue;

                    var original = property.OriginalValue?.ToString();
                    var current = property.CurrentValue?.ToString();

                    if (original == current)
                        continue;

                    oldValues[property.Metadata.Name] = original;
                    newValues[property.Metadata.Name] = current;
                }

                if (!oldValues.Any())
                    continue;
            }

            // userId
            var userIdStr = _httpContextAccessor
                .HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;

            long? userId = null;
            if (!string.IsNullOrEmpty(userIdStr))
                userId = long.Parse(userIdStr);

            // ip
            var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            histories.Add(
                new HistoryEntity
                {
                    UserId = userId,
                    TableName = entry.Metadata.GetTableName(),
                    RecordId = recordId,
                    ActionType = entry.State switch
                    {
                        EntityState.Added => ActionTypeMap.CREATE,
                        EntityState.Modified => ActionTypeMap.UPDATE,
                        EntityState.Deleted => ActionTypeMap.DELETE,
                        _ => ActionTypeMap.UPDATE,
                    },
                    OldValues = oldValues.Any() ? JsonSerializer.Serialize(oldValues) : null,
                    NewValues = newValues.Any() ? JsonSerializer.Serialize(newValues) : null,
                    IpAddress = ip,
                }
            );
        }

        if (histories.Any())
            context.Set<HistoryEntity>().AddRange(histories);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AuditIgnoreAttribute : Attribute { }
}
