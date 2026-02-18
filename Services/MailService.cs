
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using salian_api.Config.Extentions;
using salian_api.Dtos.Email;

namespace salian_api.Services
{
    public class MailService : Interface.IMailService
    {
        private readonly MailSettings _mailSettings;
       /* Injecting the IOptions to the constructor and assigning it’s value to the instance of MailSettings.
            we will be able to access the data from the JSON at runtime*/
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmail(SendMailDto mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.Sender.Name = _mailSettings.DisplayName;
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        // Use this way
       /* public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }*/
    }
}
