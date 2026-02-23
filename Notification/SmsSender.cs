using Kavenegar;
using Microsoft.Extensions.Options;
using salian_api.Config.SMS;

namespace salian_api.Notification
{
    public class SmsSender(IOptions<KavenegarSettings> _kavenegarSetting)
    {
        public async Task SendSmsWithToken(string mobile, string token, string template = "Login")
        {
            try
            {
                var api = new KavenegarApi(_kavenegarSetting.Value.ApiKey);
                var result = api.VerifyLookup(mobile, token, template);

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
