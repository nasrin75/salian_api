using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using salian_api.Dtos.Auth;
using salian_api.Helper;
using salian_api.Infrastructure.Data;
using salian_api.Notification;
using salian_api.Response;

namespace salian_api.Services.Password
{
    public class PasswordService : IPasswordService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IMemoryCache _cache;
        private readonly MailSender _mailSender;

        public PasswordService(
            IBackgroundJobClient backgroundJobClient,
            ApplicationDbContext dbContext,
            MailSender mailSender,
            IMemoryCache cache
        )
        {
            _backgroundJobClient = backgroundJobClient;
            _dbContext = dbContext;
            _mailSender = mailSender;
            _cache = cache;
        }

        public async Task<BaseResponse> SendResetPasswordByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (string.IsNullOrWhiteSpace(email) || user == null)
                return new BaseResponse(400, "EMAIL_NOT_EXIST");

            // check if send code stop until expire token time
            var cache = _cache.Get<string>($"reset_password:user_id:{user.Id}");
            if (!string.IsNullOrWhiteSpace(cache))
                return new BaseResponse(400, "TOKEN_TIME_REMAINED");

            GenerateTokenAndNotify(user.Id, email);

            return new BaseResponse();
        }

        public async Task<BaseResponse> VerifyResetPasswordByEmail(VerifyResetPasswordDto request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return new BaseResponse(400, "EMAIL_NOT_EXIST");

            //check token is valid or not
            var token = _cache.Get<string>($"reset_password:user_id:{user.Id}");
            if (token == request.Token)
            {
                user.Password = PasswordHelper.HashPassword(request.NewPassword);

                await _dbContext.SaveChangesAsync();

                //remove cache
                _cache.Remove($"reset_password:user_id:{user.Id}");

                return new BaseResponse();
            }
            return new BaseResponse(400, "TOKEN_IS_INVALID");
        }

        private void GenerateTokenAndNotify(long userId, string email)
        {
            // create token
            var token = CommonHelper.GenerateToken();

            // cache token to after verify
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(33),
            };
            _cache.Set<String>($"reset_password:user_id:{userId}", token, cacheOptions);

            var body = "کد بازیابی رمز عبور شما :: " + token;

            // send Email
            _backgroundJobClient.Enqueue(() =>
                _mailSender.SendMail(email, "بازیابی رمزعبور", body)
            );
        }
    }
}
