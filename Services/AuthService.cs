using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using salian_api.Config;
using salian_api.Config.Permissions;
using salian_api.Dtos.Auth;
using salian_api.Dtos.Otp;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Response.Auth;
using salian_api.Response.Otp;
using salian_api.Routes;

namespace salian_api.Services
{
    public class AuthService(ApplicationDbContext _dbContext, IOptions<AuthSettings> _authSetting)
        : IAuthService
    {
        public async Task<BaseResponse<LoginResponse>> Login(LoginDto request)
        {
            var user = await _dbContext
                .Users.Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.Username == request.Username && x.Password == request.Password
                );

            // return null if user not found
            if (user == null)
                return new BaseResponse<LoginResponse>(null, 400, "USER_NOT_FOUND");

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

            var token = GenerateJwtToken(user);

            var response = new LoginResponse
            {
                UserId = user.Id,
                Role = user.Role.EnName,
                Username = request.Username,
                Token = token,
            };

            return new BaseResponse<LoginResponse>(response);
        }

        public async Task<BaseResponse<OtpResponse>> SendOtp(SendOtpDto request)
        {
            var user = await _dbContext
                .Users.Where(x =>
                    (
                        x.Username == request.Username
                        || x.Email == request.Username
                        || x.Mobile == request.Username
                    )
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

        /*     public async Task<List<string>> GetUserPermissions(long userId)
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
             }*/

        private string GenerateJwtToken(UserEntity user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSetting.Value.Secret);
            var expireDate = DateTime.UtcNow.AddMinutes(30);

            var claims = new ClaimsIdentity();
            // add claims
            foreach (var permission in user.Permissions)
            {
                claims.AddClaims(new[] { new Claim(Permissions.Permission, permission.Name) });
            }

            // var rolePermissions = _dbContext
            //     .Roles.Include(r => r.Permissions)
            //     .FirstOrDefaultAsync(role => role.Id == user.RoleId)
            //     .Permissions;

            foreach (var rolePermission in user.Role.Permissions)
            {
                claims.AddClaims(new[] { new Claim(Permissions.Permission, rolePermission.Name) });
            }
            // claims.AddClaims(
            //     new[]
            //     {
            //         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //         new Claim(ClaimTypes.Name, user.Username!),
            //         new Claim(ClaimTypes.Email, user.Email!),
            //         new Claim(ClaimTypes.MobilePhone, user.Mobile!),
            //         new Claim(ClaimTypes.Role, user.Role.EnName.ToString()),
            //     }
            // );

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
