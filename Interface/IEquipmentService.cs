using salian_api.Dtos.Equipment;
using salian_api.Response;

namespace salian_api.Interface
{
    public interface IEquipmentService
    {
        Task<BaseResponse<EquipmentResponse>> Create(EquipmentCreateDto param);
        Task<BaseResponse<EquipmentResponse?>> Update(EquipmentUpdateDto param);
        Task<BaseResponse<EquipmentResponse?>> GetByID(long EquipmentID);
        Task<BaseResponse<List<EquipmentResponse>>> GetAll();
        Task<BaseResponse<List<EquipmentResponse>>> Search(SearchEquipmentDto param);

        Task<BaseResponse> Delete(long id);
    }
}
