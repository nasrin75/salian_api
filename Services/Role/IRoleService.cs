using salian_api.Dtos.Role;
using salian_api.Response;

namespace salian_api.Services.Role
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleResponse>> Create(RoleCreateDto role);
        Task<BaseResponse<RoleResponse?>> Update(RoleUpdateDto role);
        Task<BaseResponse<RoleResponse?>> GetRoleByID(long roleID);
        Task<BaseResponse<List<RoleResponse>>> GetAllRoles();
        Task<BaseResponse> Delete(long id);
        Task<BaseResponse<List<PermissionResponse>>> GetRolePermissions(long roleID);
        Task<BaseResponse> AssignPermission(AssignRolePermissionDto request);
    }
}
