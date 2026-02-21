using salian_api.Dtos.Auth;
using salian_api.Dtos.Otp;
using salian_api.Response;
using salian_api.Response.Auth;
using salian_api.Response.Otp;

namespace salian_api.Services.Auth
{
    public interface IAuthService
    {
        Task<BaseResponse<LoginResponse>> Login(LoginDto request);
        Task<BaseResponse<OtpResponse>> SendOtp(SendOtpDto request);
    }
}
