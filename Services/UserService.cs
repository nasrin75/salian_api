using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.User;
using salian_api.Entities;
using salian_api.Interface;
using System.Linq;
using System.Net.NetworkInformation;


namespace salian_api.Services
{
    public class UserService(ApplicationDbContext dbContext) : IUserService
    {
        public async Task<UserResponse> Create(UserCreateDto dto)
        {
 

            var user = new UserEntity
            {
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                Mobile = dto.Mobile,
                RoleId = dto.RoleId,
                IsCheckIp = (bool)dto.IsCheckIp,
                LoginType = (Entities.LoginTypes)dto.LoginType,
                Status = (StatusLists)dto.Status
            };

            var newUser = dbContext.Users.Add(user).Entity;
            await dbContext.SaveChangesAsync();

            var Ips = dto.IpWhiteLists.Select(item => new IpWhiteListEntity
            {
                Ip = item,
                UserId = user.Id
            }).ToList();

            dbContext.IpWhiteLists.AddRangeAsync(Ips);
            await dbContext.SaveChangesAsync();

            
            UserResponse response = new()
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                Mobile = newUser.Mobile,
                RoleId = newUser.RoleId,
                IsCheckIp = (bool)newUser.IsCheckIp,
                LoginType = (Dtos.User.LoginTypes)newUser.LoginType,
                Status = newUser.Status,
            };

            return response;
        }

        public async Task Delete(long userID)
        {
            UserEntity? user =  await dbContext.Users.FindAsync(userID);
            if (user == null) return;

            user.IsDeleted = true;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
             List <UserResponse> userList = await dbContext.Users
               .Select(u => new UserResponse
               {
                   Id = u.Id,
                   Email = u.Email,
                   Username = u.Username,
                   Mobile = u.Mobile,
                   IsCheckIp = (bool)u.IsCheckIp,
                   LoginType = (Dtos.User.LoginTypes)u.LoginType,
                   Status = u.Status,
                   RoleId = u.RoleId,
               })
                .ToListAsync();

            return new List<UserResponse>(userList);
        }

        public async Task<UserResponse?> GetUserByID(long userID)
        {
            UserEntity? user = await dbContext.Users.FindAsync(userID);

            if (user == null)
            {
                return null;
            }

            UserResponse response = new()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                IsCheckIp = (bool)user.IsCheckIp,
                LoginType = (Dtos.User.LoginTypes)user.LoginType,
                Status = user.Status,
                RoleId = user.RoleId,
            };

            return response;
        }


        public async Task<UserResponse?> Update(UserUpdateDto dto)
        {
            // Update User
           UserEntity? user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (user == null) return null;

            if (dto.Email != null) user.Email = dto.Email;
            if (dto.Mobile != null) user.Mobile = dto.Mobile;
            if (dto.Password != null) user.Password = dto.Password;
            if (dto.Username != null) user.Username = dto.Username;
            if (dto.IsCheckIp != null) user.IsCheckIp = dto.IsCheckIp.Value;
            if (dto.LoginType != null) user.LoginType = (Entities.LoginTypes)dto.LoginType;
            if (dto.Status != null) user.Status = (StatusLists)dto.Status;

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();

            // Update IpWhitList
            var ExistingIps = dbContext.IpWhiteLists.Where(x => x.UserId == dto.Id).Select(x=>x.Ip).ToList();

           /* Console.WriteLine(Existin)*/
            /*var RemovedIps = dto.IpWhiteLists.Where(x => dto.IpWhiteLists.Contains(ExistingIps));

            if (ExistingIps != null)
            {
                dbContext.IpWhiteLists.RemoveRange(ExistingIps);
                await dbContext.SaveChangesAsync();
            }*/

            var newIps = dto.IpWhiteLists.Select(item => new IpWhiteListEntity
            {
                Ip = item,
                UserId = dto.Id,
            }).ToList();

            dbContext.IpWhiteLists.AddRange(newIps);
            await dbContext.SaveChangesAsync();


            // response
            return new UserResponse()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                IsCheckIp = (bool)user.IsCheckIp,
                LoginType = (Dtos.User.LoginTypes)user.LoginType,
                Status = user.Status,
                RoleId = user.RoleId
            };
        }
    }
}
