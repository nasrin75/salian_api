using salian_api.Dtos.Permission;
using salian_api.Response;

namespace salian_api.Services.Permission
{
    public interface IPermissionService
    {
        Task<BaseResponse<PermissionResponse>> Create(PermissionCreateDto param);
        Task<BaseResponse<PermissionResponse?>> Update(PermissionUpdateDto param);
        Task<BaseResponse<PermissionResponse?>> GetByID(long id);
        Task<BaseResponse<List<PermissionResponse>>> GetAll();
        Task<BaseResponse> Delete(long id);
    }
}
