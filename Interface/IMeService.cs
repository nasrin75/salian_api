using salian_api.Response;

namespace salian_api.Interface
{
    public interface IMeService
    {
        Task<BaseResponse<List<PermissionResponse>>> MyPermissions();
    }
}
