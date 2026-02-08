using salian_api.Dtos.Profile;
using salian_api.Response;
using salian_api.Response.User;

namespace salian_api.Interface
{
    public interface IProfileService
    {
        public Task<BaseResponse<ProfileResponse>> Update(ProfileUpdateDto dto);
        public Task<BaseResponse<ProfileResponse>> GetByID(long Id);
    }
}
