using salian_api.Dtos.Auth;
using salian_api.Response;
using salian_api.Response.Auth;

namespace salian_api.Interface
{
    public interface IAuthService
    {
       public Task<BaseResponse<LoginResponse>> Login(LoginDto request);
    }
}
