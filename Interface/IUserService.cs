using salian_api.Dtos.User;
using salian_api.Entities;
using salian_api.Response;

namespace salian_api.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<UserResponse>> Create(UserCreateDto user);
        Task<BaseResponse<UserResponse?>> Update(UserUpdateDto user);
        Task<BaseResponse<UserResponse?>> GetUserByID(long userID);
        Task<BaseResponse<List<UserResponse>>> GetAllUsers();
        Task<BaseResponse> Delete(long id);
    }
}
