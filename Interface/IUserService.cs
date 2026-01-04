using salian_api.Dtos.User;
using salian_api.Entities;

namespace salian_api.Interface
{
    public interface IUserService
    {
        Task<UserResponse> Create(UserCreateDto user);
        Task<UserResponse?> Update(UserUpdateDto user);
        Task<UserResponse?> GetUserByID(long userID);
        Task<List<UserResponse>> GetAllUsers();
        Task Delete(long id);
    }
}
