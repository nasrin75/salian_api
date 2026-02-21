using System;
using Azure;
using Kavenegar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using salian_api.Config.SMS;
using salian_api.Dtos.Otp;
using salian_api.Infrastructure.Data;
using salian_api.Response;
using salian_api.Response.Otp;

namespace salian_api.Services.Sms
{
    public class SmsService(
        ApplicationDbContext _dbContext,
        IOptions<KavenegarSettings> _kavenegarSetting
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

            /* 

              foreach (var item in user.LoginTypes)
              {
                  /* if (item === "otp")
                   {
                        SendSms(user.Mobile);
                   }
              }
        */
            return new BaseResponse<OtpResponse>(null, 200, user.Mobile);
        }

        private void SendSms(string mobile)
        {
            try
            {
                var token = "123555"; // TODO:generate it
                var api = new KavenegarApi(_kavenegarSetting.Value.ApiKey);
                var result = api.VerifyLookup(mobile, token, "Login");
                Console.WriteLine("kavenegar_resp :: " + result);
                //api.Send(sender, receptor, message); //Note: send sms without token
            }
            catch (Kavenegar.Exceptions.ApiException ex)
            {
                // when response status code != 200
                Console.Write("Message : " + ex.Message);
            }
            catch (Kavenegar.Exceptions.HttpException ex)
            {
                // When accure network connection
                Console.Write("Message : " + ex.Message);
            }
        }
    }
}
