using salian_api.Dtos.History;
using salian_api.Response;
using salian_api.Response.History;

namespace salian_api.Services.History
{
    public interface IHistoryService
    {
        Task<BaseResponse<List<HistoryListResponse>>> GetAllHistory(
            HistorySearchParamsDto requeest
        );
        Task<BaseResponse<HistoryResponse>> GetHistory(long id);
        Task<BaseResponse<List<HistoryListResponse>>> GetByEntity(string entity, long entityId);
        Task<BaseResponse> Delete(long id);
    }
}
