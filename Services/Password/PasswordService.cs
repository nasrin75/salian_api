using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MimeKit;
using salian_api.Config.Extentions;
using salian_api.Dtos.Auth;
using salian_api.Helper;
using salian_api.Infrastructure.Data;
using salian_api.Response;

namespace salian_api.Services.Password
{
    public class PasswordService : IPasswordService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MailSettings _mailSettings;
        private readonly IMemoryCache _cache;


        public PasswordService(IOptions<MailSettings> mailSettings,
            ApplicationDbContext dbContext,
            IMemoryCache cache
            )
        {
            _mailSettings = mailSettings.Value;
            _dbContext = dbContext;
            _cache = cache;
        }
        public async Task<BaseResponse> SendResetPasswordByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (string.IsNullOrWhiteSpace(email) || user == null )
                return new BaseResponse(400, "EMAIL_NOT_EXIST");

            var cache = _cache.Get<string>($"reset_password:user_id:{user.Id}");
            if (!string.IsNullOrWhiteSpace(cache) ) return new BaseResponse(400, "TOKEN_TIME_REMAINED");

            //TODO:have to create token and check don't expire time
            //
            var random = new Random();
            var token = random.Next(99999, 999999);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(33),
            };
            // Save data in cache
            _cache.Set<String>($"reset_password:user_id:{user.Id}", token.ToString(), cacheOptions);

            Console.WriteLine("cache is::: " + token);
            var body = "کد بازیابی رمز عبور شما :: " + token;

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(
                new MailboxAddress(_mailSettings.FromName, _mailSettings.FromEmail)
            );
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "بازیابی رمزعبور";

            var bodyBuilder = new BodyBuilder { HtmlBody = body };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, false);
                await client.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            return new BaseResponse();
        }

        public async Task<BaseResponse> VerifyResetPasswordByEmail(VerifyResetPasswordDto request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return new BaseResponse(400, "EMAIL_NOT_EXIST");

            var token = _cache.Get<string>($"reset_password:user_id:{user.Id}");

            Console.WriteLine("token" + token + "req::" + request.Token + " :: userID::" + $"reset_password:user_id:{user.Id}");

            if(token == request.Token)
            {
                user.Password = PasswordHelper.HashPassword(request.NewPassword) ;

               await _dbContext.SaveChangesAsync();

                //remove cache
                _cache.Remove($"reset_password:user_id:{user.Id}");

                return new BaseResponse();
            }
            return new BaseResponse(400, "TOKEN_IS_INVALID");

        }

    }
}
