using salian_api.Response;

namespace salian_api.Services.History
{
    public interface IHistoryService
    {
        Task<BaseResponse<List<HistoryResponse>>> GetAllHistory();
        Task<BaseResponse<List<HistoryResponse>>> GetByEntity(string entity, long entityId);
    }
}
