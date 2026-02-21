using salian_api.Response;

namespace salian_api.Services.Me
{
    public interface IMeService
    {
        Task<BaseResponse<List<PermissionResponse>>> MyPermissions();
    }
}
