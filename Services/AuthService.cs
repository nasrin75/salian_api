using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using salian_api.Config;
using salian_api.Dtos.Auth;
using salian_api.Dtos.Otp;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Response.Auth;
using salian_api.Response.Otp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace salian_api.Services
{
    public class AuthService(ApplicationDbContext _dbContext, IOptions<AuthSettings> _authSetting) : IAuthService
    {

        public async Task<BaseResponse<LoginResponse>> Login(LoginDto request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password);

            // return null if user not found
            if (user == null)
                return new BaseResponse<LoginResponse>(null, 400, "USER_NOT_FOUND");

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSetting.Value.Secret);

            var claims = new ClaimsIdentity();
            claims.AddClaims(new[]
            {
                   new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                   new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
               });

            var expireDate = DateTime.UtcNow.AddMinutes(30);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var response = new LoginResponse
            {
                UserId = user.Id,
                Username = request.Username,
                Token = tokenHandler.WriteToken(token),
                ExpireDate = expireDate.ToString(),
            };


            return new BaseResponse<LoginResponse>(response);
        }


       // public async Task<BaseResponse<OtpResponse>> SendOtp(SendOtpDto request)
        public async Task<BaseResponse<OtpResponse>> SendOtp(SendOtpDto request)
        {
            var user = await _dbContext.Users.Where(x => (
            x.Username == request.Username ||
                    x.Email == request.Username ||
                    x.Mobile == request.Username
                    ))
                .FirstOrDefaultAsync();

            if (user == null) return new BaseResponse<OtpResponse>(null,400, "USER_NOT_FOUND");
            //return new BaseResponse<UserEntity>(user, 400, "USER_NOT_FOUND");
            
            foreach (var item in user.LoginTypes)
            {
               /* if (item === "otp")
                {
                    Send
                }*/
            }
            return new BaseResponse<OtpResponse>(null, 200, "SENT_OTP");
        }
    }
}
