using salian_api.Dtos.Role;
using salian_api.Dtos.User;
using salian_api.Response;

namespace salian_api.Interface
{
    public interface IRoleService
    {
        Task<List<RoleResponse>> GetAllRoles();
        Task<RoleResponse?> GetRoleByID(long RoleID);
        Task<RoleResponse> Create(RoleCreateDto dto);
        Task<RoleResponse?> Update(RoleUpdateDto dto);
        Task Delete(long id);
    }
}
