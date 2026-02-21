using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.ActionType;
using salian_api.Entities;
using salian_api.Infrastructure.Data;
using salian_api.Response;

namespace salian_api.Services.ActionType
{
    public class ActionTypeService(ApplicationDbContext _dbContex) : IActionTypeService
    {
        public async Task<BaseResponse<ActionTypeResponse?>> Create(ActionTypeCreateDto param)
        {
            try
            {
                ActionTypeEntity actionType = new ActionTypeEntity
                {
                    FaName = param.FaName,
                    EnName = param.EnName,
                    IsShow = param.IsShow,
                };

                var newActionType = _dbContex.ActionTypes.Add(actionType).Entity;
                await _dbContex.SaveChangesAsync();

                ActionTypeResponse response = new ActionTypeResponse
                {
                    Id = newActionType.Id,
                    FaName = newActionType.FaName,
                    EnName = newActionType.EnName,
                    IsShow = newActionType.IsShow,
                };

                return new BaseResponse<ActionTypeResponse>(response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ActionTypeResponse>(null, 400, ex.Message);
            }
        }

        public async Task<BaseResponse> Delete(long id)
        {
            var actionType = await _dbContex.ActionTypes.FirstOrDefaultAsync(l => l.Id == id);
            if (actionType == null)
                return new BaseResponse<ActionTypeResponse?>(null, 400, "ACTION_TYPE_NOT_FOUND");

            actionType.DeletedAt = DateTime.UtcNow;

            _dbContex.ActionTypes.Update(actionType);
            await _dbContex.SaveChangesAsync();

            return new BaseResponse<ActionTypeResponse?>(
                null,
                200,
                "ActionType Successfully Is Deleted"
            );
        }

        public async Task<BaseResponse<List<ActionTypeResponse>>> GetAll()
        {
            List<ActionTypeResponse> actionTypes = await _dbContex
                .ActionTypes.AsNoTracking()
                .Select(l => new ActionTypeResponse
                {
                    Id = l.Id,
                    FaName = l.FaName,
                    EnName = l.EnName,
                    IsShow = l.IsShow,
                })
                .ToListAsync();

            return new BaseResponse<List<ActionTypeResponse>>(actionTypes);
        }

        public async Task<BaseResponse<ActionTypeResponse?>> GetByID(long ActionTypeID)
        {
            var actionType = await _dbContex
                .ActionTypes.AsNoTracking()
                .Select(item => new ActionTypeResponse
                {
                    Id = item.Id,
                    FaName = item.FaName,
                    EnName = item.EnName,
                    IsShow = item.IsShow,
                })
                .FirstOrDefaultAsync(l => l.Id == ActionTypeID);

            if (actionType == null)
                return new BaseResponse<ActionTypeResponse?>(null, 400, "ACTION_TYPE_NOT_FOUND");

            return new BaseResponse<ActionTypeResponse>(actionType);
        }

        public async Task<BaseResponse<List<ActionTypeResponse>>> Search(SearchActionTypeDto param)
        {
            var query = _dbContex.ActionTypes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(param.FaName))
                query = query.Where(x => x.FaName.Contains(param.FaName));
            if (!string.IsNullOrWhiteSpace(param.EnName))
                query = query.Where(x => x.EnName.Contains(param.EnName));

            List<ActionTypeResponse> actionTypes = await query
                .Select(l => new ActionTypeResponse
                {
                    Id = l.Id,
                    FaName = l.FaName,
                    EnName = l.EnName,
                    IsShow = l.IsShow,
                })
                .ToListAsync();

            return new BaseResponse<List<ActionTypeResponse>>(actionTypes);
        }

        public async Task<BaseResponse<ActionTypeResponse?>> Update(ActionTypeUpdateDto param)
        {
            var actionType = await _dbContex.ActionTypes.FirstOrDefaultAsync(l => l.Id == param.Id);

            if (actionType == null)
                return new BaseResponse<ActionTypeResponse?>(null, 400, "ACTION_TYPE_NOT_FOUND");

            if (param.FaName != null && param.FaName != actionType.FaName)
                actionType.FaName = param.FaName;
            if (param.EnName != null && param.EnName != actionType.EnName)
                actionType.EnName = param.EnName;
            if (param.IsShow != null && param.IsShow != actionType.IsShow)
                actionType.IsShow = param.IsShow.Value;

            _dbContex.Update(actionType);
            await _dbContex.SaveChangesAsync();

            var response = new ActionTypeResponse
            {
                Id = actionType.Id,
                FaName = actionType.FaName,
                EnName = actionType.EnName,
                IsShow = actionType.IsShow,
            };

            return new BaseResponse<ActionTypeResponse?>(response);
        }
    }
}
