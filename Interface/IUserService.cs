using salian_api.Dtos.User;
using salian_api.Response;

namespace salian_api.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<UserResponse>> Create(CreateDto user);
        Task<BaseResponse<UserResponse?>> Update(UpdateDto user);
        Task<BaseResponse<UserResponse?>> GetUserByID(long userID);
        Task<BaseResponse<List<UserResponse>>> GetAllUsers();
        Task<BaseResponse> Delete(long id);
    }
}
