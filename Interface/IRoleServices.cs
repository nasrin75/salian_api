using salian_api.Contracts.Role;
using salian_api.Models;

namespace salian_api.Interface
{
    public interface IRoleServices
    {
        Task<IEnumerable<Role>> GetRoles();
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int id);
        Task CreateRoleAsync(CreateRequest request);
        Task UpdateRoleAsync(int id,UpdateRequest  request);
        Task DeleteRoleAsync(int id);
    }
}
