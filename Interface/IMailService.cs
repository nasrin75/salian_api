using salian_api.Dtos.Email;

namespace salian_api.Interface
{
    public interface IMailService
    {
        Task SendEmail(SendMailDto mailRequest);
    }
}
