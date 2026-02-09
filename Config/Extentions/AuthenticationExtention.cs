using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;

namespace salian_api.Config.Extentions
{
    public static class AuthenticationExtention
    {

        public static IServiceCollection AddOurAuthentication(this IServiceCollection services,AuthSettings authSetting)
        {
            // Authorization service
            /* services.AddAuthorization(options =>
             {
                 options.AddPolicy("GetAllUser",
                     policy => policy.RequireClaim("AccessAllUser", "True"));
             });*/

            services.AddAuthorization();

            // JWT Authentication
            Console.WriteLine("appSet :" + authSetting);

            var key = Encoding.ASCII.GetBytes(authSetting.Secret);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            return services;
        }
    }
}
