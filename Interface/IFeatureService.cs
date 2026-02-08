using salian_api.Dtos.Feature;
using salian_api.Response;
using salian_api.Response.Feature;

namespace salian_api.Interface
{
    public interface IFeatureService
    {
        Task<BaseResponse<FeatureResponse>> Create(FeatureCreateDto param);
        Task<BaseResponse<FeatureResponse?>> Update(FeatureUpdateDto param);
        Task<BaseResponse<FeatureResponse?>> GetByID(long FeatureID);
        Task<BaseResponse<List<FeatureListResponse>>> GetAll();
        Task<BaseResponse<List<FeatureResponse>>> Search(SearchFeatureDto param);

        Task<BaseResponse> Delete(long id);
    }
}
