using salian_api.Dtos.Auth;
using salian_api.Response;

namespace salian_api.Services.Password
{
    public interface IPasswordService
    {
        Task<BaseResponse> SendResetPasswordByEmail(string email);
        Task<BaseResponse> VerifyResetPasswordByEmail(VerifyResetPasswordDto request);
    }
}
