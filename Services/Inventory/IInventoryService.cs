using Microsoft.AspNetCore.Mvc;
using salian_api.Dtos.Inventory;
using salian_api.Response;
using salian_api.Response.Inventory;

namespace salian_api.Services.Inventory
{
    public interface IInventoryService
    {
        Task<BaseResponse<InventoryResponse>> Create(InventoryCreateDto param);
        Task<BaseResponse<InventoryResponse?>> Update(InventoryUpdateDto param);
        Task<BaseResponse<InventoryResponse?>> GetByID(long InventoryID);
        Task<BaseResponse<List<InventoryListResponse>>> GetAll(string? equipment);
        Task<BaseResponse<List<InventoryListResponse>>> Search(SearchInventoryDto param);
        Task<BaseResponse> Delete(long id);
    }
}
