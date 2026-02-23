
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using salian_api.Config.Mail;
using salian_api.Dtos.Email;

namespace salian_api.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        /* Injecting the IOptions to the constructor and assigning it’s value to the instance of MailSettings.
             we will be able to access the data from the JSON at runtime*/
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmail(SendMailDto mailRequrst)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(
                new MailboxAddress(_mailSettings.FromName, _mailSettings.FromEmail)
            );
            emailMessage.To.Add(new MailboxAddress("", mailRequrst.ToEmail));
            emailMessage.Subject = mailRequrst.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = mailRequrst.Body };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, false);
                await client.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
