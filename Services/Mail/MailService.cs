using salian_api.Dtos.Email;
using salian_api.Notification;
using salian_api.Response;

namespace salian_api.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSender _mailSender;

        /* Injecting the IOptions to the constructor and assigning it’s value to the instance of MailSettings.
             we will be able to access the data from the JSON at runtime*/
        public MailService(MailSender mailSender)
        {
            _mailSender = mailSender;
        }

        public async Task<BaseResponse> SendEmail(SendMailDto mailRequest)
        {
            await _mailSender.SendMail(mailRequest.ToEmail, mailRequest.Subject, mailRequest.Body);
            return new BaseResponse();
        }
    }
}
