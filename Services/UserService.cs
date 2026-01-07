using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using salian_api.Dtos.User;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;


namespace salian_api.Services
{
    public class UserService(ApplicationDbContext dbContext) : IUserService
    {
        public async Task<BaseResponse<UserResponse>> Create(UserCreateDto dto)
        {
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

            var newUser = dbContext.Users.Add(user).Entity;
            await dbContext.SaveChangesAsync();

            // user send ips with , and can send Ips with range so first parse ips then create
            var paresedIps = ParseIp(dto.IpWhiteLists,user.Id);

            dbContext.IpWhiteLists.AddRangeAsync(paresedIps);
            await dbContext.SaveChangesAsync();

            // return response
            List<IpWhiteListResponse> whiteLists = newUser.IpWhiteLists.Select(x => new IpWhiteListResponse
            {
                Id = x.Id,
                Ip = x.Ip,
                IpRange = x.IpRange,
            }).ToList();

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


        public async Task<BaseResponse> Delete(long userID)
        {
            UserEntity? user =  await dbContext.Users.FindAsync(userID);
            if (user == null) return new BaseResponse<UserResponse?>(null, 400, "User Not Found");

            user.DeletedAt = DateTime.UtcNow;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse<List<UserResponse>>> GetAllUsers()
        {
            var users = await dbContext.Users
                .AsNoTracking() 
                .ToListAsync();

             var userList = users
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Email = u.Email,
                    Username = u.Username,
                    Mobile = u.Mobile,
                    IsCheckIp = (bool)u.IsCheckIp,
                    LoginTypes = u.LoginTypes.Select(x => x.ToString()).ToList(),
                    Status = u.Status,
                    RoleId = u.RoleId,
                });

            return new BaseResponse<List<UserResponse>>(new List<UserResponse>(userList));
        }

        public async Task<BaseResponse<UserResponse?>> GetUserByID(long userID)
        {
            UserEntity? user = await dbContext.Users.FindAsync(userID);

            if (user == null) return new BaseResponse<UserResponse?>(null, 400, "User Not Found");

            UserResponse response = new()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                IsCheckIp = (bool)user.IsCheckIp,
                LoginTypes = user.LoginTypes.Select(x=>x.ToString()).ToList(),
                Status = user.Status,
                RoleId = user.RoleId,
            };

            return new BaseResponse<UserResponse?>(response);
        }


        public async Task<BaseResponse<UserResponse?>> Update(UserUpdateDto dto)
        {
            // Update User
           UserEntity? user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
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

            var newUser = dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();

            // Update IpWhitList , check any of Ips change delete and if has new Ips add
            var parsedIps = ParseIp(dto.IpWhiteLists, dto.Id);
            var existeIps = await dbContext.IpWhiteLists.Where(x => x.UserId == dto.Id).ToListAsync();
            var removeIps = existeIps.Where(db => !parsedIps.Any(x => x.Ip == db.Ip && x.IpRange == db.IpRange))
                .ToList();
  
            var newIps = parsedIps.Where(db => !existeIps.Any(x => x.Ip == db.Ip && x.IpRange == db.IpRange))
                .ToList();

            if (removeIps.Any()) dbContext.IpWhiteLists.RemoveRange(removeIps);
            if (newIps.Any())  dbContext.IpWhiteLists.AddRange(newIps);

            await dbContext.SaveChangesAsync();

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


        private static List<IpWhiteListEntity> ParseIp(string ipWhiteLists, long userID)
        {
            var result = new List<IpWhiteListEntity>();

            if (ipWhiteLists.IsNullOrEmpty()) return result;

            var IpLists = ipWhiteLists.Split(",",StringSplitOptions.RemoveEmptyEntries);

            foreach (var ip in IpLists)
            {
                var item = ip.Trim();
                if(item.Contains(":") )
                {
                    var explodedIp = item.Split(':');
                    if(explodedIp.Length == 2 )
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
    }
}
