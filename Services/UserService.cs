using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using salian_api.Dtos.User;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Response.User;


namespace salian_api.Services
{
    public class UserService(ApplicationDbContext _dbContext, IHttpContextAccessor _httpContextAccessor) : IUserService
    {
        public async Task<BaseResponse<UserResponse>> Create(UserCreateDto dto)
        {
            var existUsername = _dbContext.Users
                .AsNoTracking()
                .Where(u => u.Username.ToLower().Trim() == dto.Username.ToLower().Trim())
                .Count();
            if (existUsername > 0) return new BaseResponse<UserResponse>(null, 400, "USERNAME_IS_EXIST");

            var existEmail = _dbContext.Users
                .AsNoTracking()
                .Where(u => u.Email.ToLower().Trim() == dto.Email.ToLower().Trim())
                .Count();
            if (existEmail > 0) return new BaseResponse<UserResponse>(null, 400, "EAMIL_IS_EXIST");

            // convert dto.loginType to LoginTypes's enum, if this string dosen't exist in this enum get exception 
            var LoginTypes = dto.LoginTypes
                .Select(x => Enum.Parse<LoginTypes>(x))
                .ToList();

           // first create user then asign whitelists
            var user = new UserEntity
            {
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                Mobile = dto.Mobile,
                RoleId = dto.RoleId,
                IsCheckIp = (bool)dto.IsCheckIp,
                LoginTypes = LoginTypes,
                Status = (StatusLists)dto.Status
            };

            var newUser = _dbContext.Users.Add(user).Entity;
            await _dbContext.SaveChangesAsync();

            // user send ips with , and can send Ips with range so first parse ips then create
            List<IpWhiteListResponse> whiteLists = [];
            if (!string.IsNullOrEmpty(dto.IpWhiteLists))
            {
                var paresedIps = ParseIp(dto.IpWhiteLists, user.Id);

                _dbContext.IpWhiteLists.AddRangeAsync(paresedIps);

                await _dbContext.SaveChangesAsync();
                // return response
                whiteLists = newUser.IpWhiteLists.Select(x => new IpWhiteListResponse
                {
                    Id = x.Id,
                    Ip = x.Ip,
                    IpRange = x.IpRange,
                }).ToList();
            }
           

           UserResponse response = new()
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                Mobile = newUser.Mobile,
                RoleId = newUser.RoleId,
                IsCheckIp = (bool)newUser.IsCheckIp,
                LoginTypes = LoginTypes.Select(x=>x.ToString()).ToList(),
                Status = newUser.Status,
                IpWhiteLists = whiteLists
           };

            return new BaseResponse<UserResponse>(response);
        }


        public async Task<BaseResponse> Delete(long id)
        {
            UserEntity? user =  await _dbContext.Users.FindAsync(id);
            if (user == null) return new BaseResponse<UserResponse?>(null, 400, "User Not Found");

            user.DeletedAt = DateTime.UtcNow;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse<List<UserListResponse>>> GetAllUsers()
        {
            List<UserListResponse> userList = await _dbContext.Users
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                 .Select(u => new UserListResponse
                 {
                     Id = u.Id,
                     Email = u.Email,
                     Username = u.Username,
                     Mobile = u.Mobile,
                     Status = (int)u.Status == 1 ? "Active" : "Deactive",
                     Role = u.Role.FaName != null ? u.Role.FaName : u.Role.EnName
                 })
                .ToListAsync();

            return new BaseResponse<List<UserListResponse>>(new List<UserListResponse>(userList));
        }

        public async Task<BaseResponse<UserResponse?>> GetUserByID(long userID)
        {
            UserEntity? user = await _dbContext.Users
                .Include(x =>x.IpWhiteLists)
                .FirstOrDefaultAsync(x => x.Id ==userID);

            if (user == null) return new BaseResponse<UserResponse?>(null, 400, "User Not Found");

            List<IpWhiteListResponse> ips = [];
            if (user.IpWhiteLists != null)
            {
                ips = user.IpWhiteLists.Select(x => new IpWhiteListResponse
                {
                    Id = x.Id,
                    Ip = x.Ip,
                    IpRange = x.IpRange,
                }).ToList();
            }
            UserResponse response = new()
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Mobile = user.Mobile,
                IsCheckIp = (bool)user.IsCheckIp,
                LoginTypes = user.LoginTypes.Select(x=>x.ToString()).ToList(),
                Status = user.Status,
                RoleId = user.RoleId,
                IpWhiteLists = user.IpWhiteLists.Select(x => new IpWhiteListResponse
                {
                    Id = x.Id,
                    Ip = x.Ip,
                    IpRange = x.IpRange,
                }).ToList(),
            };

            return new BaseResponse<UserResponse?>(response);
        }


        public async Task<BaseResponse<UserResponse?>> Update(UserUpdateDto dto)
        {
            // Update User
           UserEntity? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (user == null) return new BaseResponse<UserResponse?>(null,400, "User Not Found");

            if (dto.Email != null) user.Email = dto.Email;
            if (dto.Mobile != null) user.Mobile = dto.Mobile;
            if (dto.Password != null) user.Password = dto.Password;
            if (dto.Username != null) user.Username = dto.Username;
            if (dto.IsCheckIp != null) user.IsCheckIp = dto.IsCheckIp.Value;
            if (dto.LoginTypes != null) user.LoginTypes =  dto.LoginTypes
                .Select(x => Enum.Parse<LoginTypes>(x)) // true برای case-insensitive
                .ToList();;
            if (dto.Status != null) user.Status = (StatusLists)dto.Status;

            var newUser = _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            // Update IpWhitList , check any of Ips change delete and if has new Ips add
            var parsedIps = ParseIp(dto.IpWhiteLists, dto.Id);
            var existeIps = await _dbContext.IpWhiteLists.Where(x => x.UserId == dto.Id).ToListAsync();
            var removeIps = existeIps.Where(db => !parsedIps.Any(x => x.Ip == db.Ip && x.IpRange == db.IpRange))
                .ToList();
  
            var newIps = parsedIps.Where(db => !existeIps.Any(x => x.Ip == db.Ip && x.IpRange == db.IpRange))
                .ToList();

            if (removeIps.Any()) _dbContext.IpWhiteLists.RemoveRange(removeIps);
            if (newIps.Any()) _dbContext.IpWhiteLists.AddRange(newIps);

            await _dbContext.SaveChangesAsync();

            // response
            List<IpWhiteListResponse> whiteLists = user.IpWhiteLists.Select(x => new IpWhiteListResponse
            {
                Id = x.Id,
                Ip = x.Ip,
                IpRange = x.IpRange,
            }).ToList();

            var response = new UserResponse()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                IsCheckIp = (bool)user.IsCheckIp,
                LoginTypes = user.LoginTypes.Select(x=>x.ToString()).ToList(),
                Status = user.Status,
                RoleId = user.RoleId,
                IpWhiteLists = whiteLists,
            };

            return new BaseResponse<UserResponse>(response);
        }

        public async Task<BaseResponse<List<UserListResponse>>> Search(UserSearchDto param)
        {
            var query = _dbContext.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(param.Username)) query = query.Where(x => x.Username.Contains(param.Username));
            if (!string.IsNullOrWhiteSpace(param.Email)) query = query.Where(x => x.Email.Contains(param.Email));
            if (!string.IsNullOrWhiteSpace(param.Mobile)) query = query.Where(x => x.Mobile.Contains(param.Mobile));

            List<UserListResponse> users = await query.Select(u => new UserListResponse
            {
                Id = u.Id,
                Email = u.Email,
                Username = u.Username,
                Mobile = u.Mobile,
                Status = (int)u.Status == 1 ? "Active" : "Deactive",
                Role = u.Role.FaName != null ? u.Role.FaName : u.Role.EnName
            }).ToListAsync();

            return new BaseResponse<List<UserListResponse>>(users);
        }

        private static List<IpWhiteListEntity> ParseIp(string ipWhiteLists, long userID)
        {
            var result = new List<IpWhiteListEntity>();

            if (ipWhiteLists.IsNullOrEmpty()) return result;

            var IpLists = ipWhiteLists.Split(",", StringSplitOptions.RemoveEmptyEntries);

            foreach (var ip in IpLists)
            {
                var item = ip.Trim();
                if (item.Contains(":"))
                {
                    var explodedIp = item.Split(':');
                    if (explodedIp.Length == 2)
                    {
                        result.Add(new IpWhiteListEntity
                        {
                            Ip = explodedIp[0],
                            IpRange = explodedIp[1],
                            UserId = userID
                        });
                    }

                }
                else
                {
                    result.Add(new IpWhiteListEntity
                    {
                        Ip = item,
                        UserId = userID
                    });
                }
            }

            return result;
        }

        public async Task<BaseResponse<List<PermissionResponse>>> UserPermissions()
        {

            var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userID == null) return new BaseResponse<List<PermissionResponse>>(null, 400, "ACCESS_DENY");

            UserEntity user = await _dbContext.Users
                .Include("Permissions")
                .FirstOrDefaultAsync(x => x.Id.ToString() == userID);
            if (user == null) return new BaseResponse<List<PermissionResponse>>(null, 400, "USER_NOT_FOUND");

            var permissions = user.Permissions.Select(p => new PermissionResponse
            {
                Id = p.Id,
                Name = p.Name,
            }).ToList();
            return new BaseResponse<List<PermissionResponse>>(permissions);
        }
    }
}
