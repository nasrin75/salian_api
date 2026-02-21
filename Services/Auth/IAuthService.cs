using salian_api.Dtos.Auth;
using salian_api.Response;
using salian_api.Response.Auth;

namespace salian_api.Services.Auth
{
    public interface IAuthService
    {
        Task<BaseResponse<LoginResponse>> Login(LoginDto request);
        Task<BaseResponse> SendResetPasswordEmail(string email);
        Task<BaseResponse> VerifyResetPasswordEmail(VerifyResetPasswordDto request);
    }
}
