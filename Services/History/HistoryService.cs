using Microsoft.EntityFrameworkCore;
using salian_api.Infrastructure.Data;
using salian_api.Response;
using salian_api.Response.User;

namespace salian_api.Services.History
{
    public class HistoryService(ApplicationDbContext _dbContext) : IHistoryService
    {
        public async Task<BaseResponse<List<HistoryResponse>>> GetAllHistory()
        {
            var histories = await _dbContext
                .Histories.AsNoTracking()
                .Include(h => h.User)
                .Select(h => new HistoryResponse
                {
                    Id = h.Id,
                    ActionType = h.ActionType,
                    CreatedAt = h.CreatedAt,
                    ChangedData = h.OldValues,
                    Ip = h.IpAddress,
                    Entity = h.TableName,
                    EntityId = h.RecordId,
                    User = h.User.Username,
                })
                .ToListAsync();

            return new BaseResponse<List<HistoryResponse>>(histories);
        }

        public async Task<BaseResponse<List<HistoryResponse>>> GetByEntity(
            string entity,
            long entityId
        )
        {
            var histories = await _dbContext
                .Histories.AsNoTracking()
                .Include(h => h.User)
                .Where(h => h.RecordId == entityId)
                .Where(h => h.TableName.ToLower() == entity.ToLower())
                .Select(h => new HistoryResponse
                {
                    Id = h.Id,
                    ActionType = h.ActionType,
                    CreatedAt = h.CreatedAt,
                    ChangedData = h.OldValues,
                    Ip = h.IpAddress,
                    Entity = h.TableName,
                    EntityId = h.RecordId,
                    User = h.User.Username,
                })
                .ToListAsync();

            return new BaseResponse<List<HistoryResponse>>(histories);
        }
    }
}
