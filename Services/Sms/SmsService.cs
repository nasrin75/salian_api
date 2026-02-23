using Hangfire;
using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Otp;
using salian_api.Helper;
using salian_api.Infrastructure.Data;
using salian_api.Notification;
using salian_api.Response;
using salian_api.Response.Otp;

namespace salian_api.Services.Sms
{
    public class SmsService(
        ApplicationDbContext _dbContext,
        IBackgroundJobClient _backgroundJob,
        MailSender _mailSender,
        SmsSender _smsSender
    ) : ISmsService
    {
        public async Task<BaseResponse> SendOtp(SendOtpDto request)
        {
            var user = await _dbContext
                .Users.Where(x =>
                    x.Username == request.Username
                    || x.Email == request.Username
                    || x.Mobile == request.Username
                )
                .FirstOrDefaultAsync();

            if (user == null)
                return new BaseResponse<OtpResponse>(null, 400, "USER_NOT_FOUND");

            var token = CommonHelper.GenerateToken();
            string notificationMgs =
                $"همکار گرامی <br />  کد ورود به سیستم انبارداری IT :{token} <br />  شرکت سالیان";

            foreach (var item in user.LoginTypes)
            {
                if (item.ToString() == "email")
                {
                    _backgroundJob.Enqueue(() =>
                        _mailSender.SendMail(user.Email, "کد یکبار مصرف", notificationMgs)
                    );
                }

                if (item.ToString() == "otp")
                {
                    _backgroundJob.Enqueue(() =>
                        _smsSender.SendSmsWithToken(user.Mobile, token, "Login")
                    );
                }
            }

            return new BaseResponse<OtpResponse>(null, 200, user.Mobile);
        }
    }
}
