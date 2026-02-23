using salian_api.Dtos.Email;
using salian_api.Response;

namespace salian_api.Services.Mail
{
    public interface IMailService
    {
        Task<BaseResponse> SendEmail(SendMailDto mailRequest);
    }
}
