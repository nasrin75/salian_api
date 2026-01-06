using salian_api.Dtos.Role;
using salian_api.Response;

namespace salian_api.Interface
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleResponse>> Create(CreateDto user);
        Task<BaseResponse<RoleResponse?>> Update(UpdateDto user);
        Task<BaseResponse<RoleResponse?>> GetRoleByID(long userID);
        Task<BaseResponse<List<RoleResponse>>> GetAllRoles();
        Task<BaseResponse> Delete(long id);
    }
}
