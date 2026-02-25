using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.History;
using salian_api.Infrastructure.Data;
using salian_api.Response;
using salian_api.Response.History;

namespace salian_api.Services.History
{
    public class HistoryService(ApplicationDbContext _dbContext) : IHistoryService
    {
        public async Task<BaseResponse<List<HistoryListResponse>>> GetAllHistory(
            [FromBody] HistorySearchParamsDto request
        )
        {
            var query = _dbContext
                .Histories.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.EntityName))
            {
                query = query.Where(i => i.TableName.ToLower() == request.EntityName.ToLower());
            }
            if (request.EntityId != null)
            {
                query = query.Where(i => i.RecordId == request.EntityId);
            }

            var histories = await query.Include(h => h.User).ToListAsync();

            var response = histories
                .Select(h => new HistoryListResponse
                {
                    Id = h.Id,
                    UserId = h.UserId,
                    ActionType = h.ActionType,
                    CreatedAt = h.CreatedAt,
                    Ip = h.IpAddress,
                    NewData = string.IsNullOrWhiteSpace(h.NewValues)
                        ? null
                        : JsonSerializer.Deserialize<object>(h.NewValues),
                    OldData = string.IsNullOrWhiteSpace(h.OldValues)
                        ? null
                        : JsonSerializer.Deserialize<object>(h.OldValues),
                    Entity = h.TableName,
                    EntityId = h.RecordId,
                    User = h.User?.Username,
                })
                .ToList();

            return new BaseResponse<List<HistoryListResponse>>(response);
        }

        public async Task<BaseResponse<List<HistoryListResponse>>> GetByEntity(
            string entity,
            long entityId
        )
        {
            var histories = await _dbContext
                .Histories.AsNoTracking()
                .Include(h => h.User)
                .Where(h => h.RecordId == entityId)
                .Where(h => h.TableName.ToLower() == entity.ToLower())
                .Select(h => new HistoryListResponse
                {
                    Id = h.Id,
                    ActionType = h.ActionType,
                    CreatedAt = h.CreatedAt,
                    Ip = h.IpAddress,
                    Entity = h.TableName,
                    EntityId = h.RecordId,
                    /*NewData = string.IsNullOrWhiteSpace(h.NewValues)
                        ? null
                        : JsonSerializer.Deserialize<object>(h.NewValues),
                    OldData = string.IsNullOrWhiteSpace(h.OldValues)
                        ? null
                        : JsonSerializer.Deserialize<object>(h.OldValues),*/
                    User = h.User.Username,
                })
                .ToListAsync();

            return new BaseResponse<List<HistoryListResponse>>(histories);
        }

        public async Task<BaseResponse<HistoryResponse>> GetHistory(long id)
        {
            var history = await _dbContext
                .Histories.AsNoTracking()
                .Include(h => h.User)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (history == null)
                return new BaseResponse<HistoryResponse>(null, 400, "HISTORY_NOT_FOUND");

            var response = new HistoryResponse
            {
                Id = history.Id,
                ActionType = history.ActionType,
                CreatedAt = history.CreatedAt,
                NewData = history.OldValues,
                Ip = history.IpAddress,
                Entity = history.TableName,
                EntityId = history.RecordId,
                User = history.User?.Username,
            };
            return new BaseResponse<HistoryResponse>(response);
        }

        public async Task<BaseResponse> Delete(long id)
        {
            var history = await _dbContext.Histories.FirstOrDefaultAsync(l => l.Id == id);
            if (history == null)
                return new BaseResponse<HistoryResponse>(null, 400, "HSTORY_NOT_FOUND");

            history.DeletedAt = DateTime.UtcNow;

            _dbContext.Update(history);
            await _dbContext.SaveChangesAsync();

            return new BaseResponse<HistoryResponse>(null, 200, "History Successfully Is Deleted");
        }
    }
}
