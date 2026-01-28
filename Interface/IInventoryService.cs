using salian_api.Dtos.Inventory;
using salian_api.Response;
using salian_api.Response.Inventory;

namespace salian_api.Interface
{
    public interface IInventoryService
    {
        Task<BaseResponse<InventoryResponse>> Create(InventoryCreateDto param);
        Task<BaseResponse<InventoryResponse?>> Update(InventoryUpdateDto param);
        Task<BaseResponse<InventoryResponse?>> GetByID(long InventoryID);
        Task<BaseResponse<List<InventoryListResponse>>> GetAll();
        Task<BaseResponse<List<InventoryListResponse>>> Search(SearchInventoryDto param);

        Task<BaseResponse> Delete(long id);
    }
}
