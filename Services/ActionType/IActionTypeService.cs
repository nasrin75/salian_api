using salian_api.Dtos.ActionType;
using salian_api.Response;

namespace salian_api.Services.ActionType
{
    public interface IActionTypeService
    {
        Task<BaseResponse<ActionTypeResponse>> Create(ActionTypeCreateDto param);
        Task<BaseResponse<ActionTypeResponse?>> Update(ActionTypeUpdateDto param);
        Task<BaseResponse<ActionTypeResponse?>> GetByID(long actionTypeID);
        Task<BaseResponse<List<ActionTypeResponse>>> GetAll();
        Task<BaseResponse<List<ActionTypeResponse>>> Search(SearchActionTypeDto param);

        Task<BaseResponse> Delete(long id);
    }
}
