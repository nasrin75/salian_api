using salian_api.Dtos.Otp;
using salian_api.Response;

namespace salian_api.Services.Sms
{
    public interface ISmsService
    {
        Task<BaseResponse> SendOtp(SendOtpDto request);
    }
}
