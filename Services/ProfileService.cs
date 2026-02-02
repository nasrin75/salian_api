using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Profile;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Services
{
    public class ProfileService(ApplicationDbContext _dbContext) : IProfileService
    {
        public async Task<BaseResponse<ProfileResponse?>> Update(ProfileUpdateDto dto)
        {
            UserEntity user = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Id == dto.Id);
            if (user == null) return new BaseResponse<ProfileResponse?>(null,400, "USER_NOT_FOUND");

            if(dto.Username !=null && dto.Username != user.Username) user.Username = dto.Username;
            if(dto.NewPassword != null && dto.NewPassword != user.Password) user.Password = dto.NewPassword;
            if(dto.Email !=null && dto.Email != user.Email) user.Email = dto.Email;
            if(dto.Mobile !=null && dto.Mobile != user.Mobile) user.Mobile = dto.Mobile;

            _dbContext.Update(user);
            _dbContext.SaveChanges();

            ProfileResponse response = new ProfileResponse
            {
                Id = user.Id,
                Email = dto.Email,
                Mobile = dto.Mobile,
                Password = dto.NewPassword,
                Username = dto.Username,

            };

            return new BaseResponse<ProfileResponse?>(response);
        }

        public async Task<BaseResponse<ProfileResponse?>> GetByID(long Id)
        {
            UserEntity user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null) return new BaseResponse<ProfileResponse?>(null, 400, "USER_NOT_FOUND");


            ProfileResponse response = new ProfileResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                Password = user.Password,
            };

            return new BaseResponse<ProfileResponse?>(response);
        }
    }
}
