using salian_api.Dtos.Equipment;
using salian_api.Response;
using salian_api.Response.Equipment;
using salian_api.Response.Feature;

namespace salian_api.Interface
{
    public interface IEquipmentService
    {
        Task<BaseResponse<EquipmentResponse>> Create(EquipmentCreateDto param);
        Task<BaseResponse<EquipmentResponse?>> Update(EquipmentUpdateDto param);
        Task<BaseResponse<EquipmentResponse?>> GetByID(long EquipmentID);
        Task<BaseResponse<List<EquipmentListResponse>>> GetAll();
        Task<BaseResponse<List<EquipmentResponse>>> Search(SearchEquipmentDto param);
        Task<BaseResponse<List<FeatureResponse>>> GetFeaturesByEquipmentID(long id);
        Task<BaseResponse<List<string>>> GetInventorySubMenu();
        Task<BaseResponse> Delete(long id);
    }
}
