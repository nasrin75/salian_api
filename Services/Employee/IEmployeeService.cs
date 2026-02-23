using salian_api.Dtos.Employee;
using salian_api.Response;

namespace salian_api.Services.Employee
{
    public interface IEmployeeService
    {
        Task<BaseResponse<EmployeeResponse>> Create(EmployeeCreateDto param);
        Task<BaseResponse<EmployeeResponse?>> Update(EmployeeUpdateDto param);
        Task<BaseResponse<EmployeeResponse?>> GetByID(long EmployeeID);
        Task<BaseResponse<List<EmployeeResponse>>> GetAll();
        Task<BaseResponse<List<EmployeeResponse>>> Search(SearchEmployeeDto param);

        Task<BaseResponse> Delete(long id);
    }
}
