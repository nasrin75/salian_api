using salian_api.Response;
using salian_api.Response.History;

namespace salian_api.Services.History
{
    public interface IHistoryService
    {
        Task<BaseResponse<List<HistoryListResponse>>> GetAllHistory();
        Task<BaseResponse<HistoryResponse>> GetHistory(long id);
        Task<BaseResponse<List<HistoryListResponse>>> GetByEntity(string entity, long entityId);
        Task<BaseResponse> Delete(long id);
    }
}
