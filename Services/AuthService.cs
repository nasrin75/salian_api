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
            var user = await _dbContext.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password);

            // return null if user not found
            if (user == null)
                return new BaseResponse<LoginResponse>(null, 400, "USER_NOT_FOUND");

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSetting.Value.Secret);

            // var claims = new ClaimsIdentity();
            // foreach (var permission in user.Permissions)
            // {
            //     claims.AddClaims(new[]
            //     {
            //         new Claim(Permissions.Permission, permission)
            //     });
            // }

            // foreach (var rolePermission in _roles.Find(role => role.Name == user.Role).Permissions)
            // {
            //     if (!user.Permissions.Any(x => x == rolePermission))
            //     {
            //         claims.AddClaims(new[]
            //         {
            //             new Claim(Permissions.Permission, rolePermission)
            //         });
            //     }
            // }

            var permissions = await GetUserPermissions(user.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }
            // claims.AddClaims(new[]
            // {
            //        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            //        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            //        new Claim(ClaimTypes.Role,user.Role.EnName)
            //    });

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

            if (user == null) return new BaseResponse<OtpResponse>(null, 400, "USER_NOT_FOUND");
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

        public async Task<List<string>> GetUserPermissions(long userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var rolePermissions = user.Role.RolePermissions
                .Select(rp => rp.Permission.Name);

            var userPermissions = user.UserPermissions
                .Select(up => up.Permission.Name);

            return rolePermissions
                .Union(userPermissions)
                .Distinct()
                .ToList();
        }
    }
}
