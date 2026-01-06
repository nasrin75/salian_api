using salian_api.Dtos.Location;
using salian_api.Response;

namespace salian_api.Interface
{
    public interface ILocationService
    {
        Task<BaseResponse<LocationResponse>> Create(LocationCreateDto param);
        Task<BaseResponse<LocationResponse?>> Update(LocationUpdateDto param);
        Task<BaseResponse<LocationResponse?>> GetByID(long locationID);
        Task<BaseResponse<List<LocationResponse>>> GetAll();
        Task<BaseResponse<List<LocationResponse>>> Search(SearchLocationDto param);

        Task<BaseResponse> Delete(long id);
    }
}
