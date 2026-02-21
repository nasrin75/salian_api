using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using salian_api.Config;
using salian_api.Config.Permissions;
using salian_api.Dtos.Auth;
using salian_api.Dtos.Otp;
using salian_api.Entities;
using salian_api.Infrastructure.Data;
using salian_api.Response;
using salian_api.Response.Auth;
using salian_api.Response.Otp;

namespace salian_api.Services.Auth
{
    public class AuthService(ApplicationDbContext _dbContext, IOptions<AuthSettings> _authSetting, IHttpContextAccessor _httpContextAccessor)
        : IAuthService
    {
        public async Task<BaseResponse<LoginResponse>> Login(LoginDto request)
        {
            var user = await _dbContext
                .Users.Include(x => x.Permissions)
                .Include(x => x.Role)
                    .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(x =>
                    x.Username == request.Username && x.Password == request.Password
                );

            // return null if user not found
            if (user == null)
                return new BaseResponse<LoginResponse>(null, 400, "USER_NOT_FOUND");

            var token = GenerateJwtToken(user);

            var response = new LoginResponse
            {
                UserId = user.Id,
                Role = user.Role.EnName,
                Username = request.Username,
                Token = token,
            };

            // add history
           /* _dbContext.Histories.Add(new HistoryEntity
            {
                UserId = user.Id,
                ActionType = ActionType.Login,
                TableName = "Users",
                RecordId = user.Id,
                IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString()
            });*/

            await _dbContext.SaveChangesAsync();

            return new BaseResponse<LoginResponse>(response);
        }

        public async Task<BaseResponse<OtpResponse>> SendOtp(SendOtpDto request)
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

            foreach (var item in user.LoginTypes)
            {
                /* if (item === "otp")
                 {
                     Send
                 }*/
            }
            return new BaseResponse<OtpResponse>(null, 200, "SENT_OTP");
        }

        private string GenerateJwtToken(UserEntity user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSetting.Value.Secret);
            var expireDate = DateTime.UtcNow.AddMinutes(30);

            var claims = new ClaimsIdentity();
            // add claims
            claims.AddClaims(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.MobilePhone, user.Mobile!),
                    new Claim(ClaimTypes.Role, user.Role.EnName.ToString()),
                }
            );
            // assign userPermissions as claims
            var userPermissions = user
                .Permissions.Select(p => new PermissionResponse { Name = p.Name })
                .ToList();
            //Console.WriteLine("user_perm:: " + userPermissions);
            foreach (var permission in userPermissions)
            {
                Console.WriteLine("user_perm:: " + permission.Name);
                claims.AddClaims(new[] { new Claim(Permissions.Permission, permission.Name) });
            }

            // assign rolePermissions as claims
            var rolePermissions = user
                .Role.Permissions.Select(p => new PermissionResponse { Name = p.Name })
                .ToList();
            foreach (var rolePermission in rolePermissions)
            {
                Console.WriteLine("role_perm:: " + rolePermission.Name);
                claims.AddClaims(new[] { new Claim(Permissions.Permission, rolePermission.Name) });
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
