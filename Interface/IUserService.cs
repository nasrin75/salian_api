using salian_api.Dtos.User;
using salian_api.Response;
using salian_api.Response.User;

namespace salian_api.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<UserResponse>> Create(UserCreateDto user);
        Task<BaseResponse<UserResponse?>> Update(UserUpdateDto user);
        Task<BaseResponse<UserResponse?>> GetUserByID(long userID);
        Task<BaseResponse<List<UserListResponse>>> GetAllUsers();
        Task<BaseResponse<List<UserListResponse>>> Search(UserSearchDto param);
        Task<BaseResponse> Delete(long id);
        Task<BaseResponse<List<PermissionResponse>>> UserPermissions();
    }
}
