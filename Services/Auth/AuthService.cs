using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using salian_api.Config;
using salian_api.Config.Extentions;
using salian_api.Config.Permissions;
using salian_api.Dtos.Auth;
using salian_api.Entities;
using salian_api.Infrastructure.Data;
using salian_api.Response;
using salian_api.Response.Auth;

namespace salian_api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly MailSettings _mailSettings;
        private readonly AuthSettings _authSetting;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            ApplicationDbContext dbContext,
            IOptions<AuthSettings> authSetting,
            IHttpContextAccessor httpContextAccessor,
            IOptions<MailSettings> mailSettings
        )
        {
            _mailSettings = mailSettings.Value;
            _dbContext = dbContext;
            _authSetting = authSetting.Value;
            _httpContextAccessor = httpContextAccessor;
        }

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

        public async Task<BaseResponse> SendResetPasswordEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return new BaseResponse(400, "EMAIL_NOT_EXIST");

            //TODO:have to create token and check don't expire time
            //
            var random = new Random();
            var token = random.Next(99999, 999999);
          /*  var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            };
            // Save data in cache
            _cache.Set(user.Id, token, cacheOptions);*/

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

        public Task<BaseResponse> VerifyResetPasswordEmail(VerifyResetPasswordDto request)
        {
            throw new NotImplementedException();
        }

        private string GenerateJwtToken(UserEntity user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSetting.Secret);
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
