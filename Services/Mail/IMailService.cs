using salian_api.Dtos.Email;

namespace salian_api.Services.Mail
{
    public interface IMailService
    {
        Task SendEmail(SendMailDto mailRequest);
    }
}
