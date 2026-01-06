using salian_api.Dtos.Employee;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Routes
{
    public static class Employee 
    {
        public static void MapEmployeeRoutes(this IEndpointRouteBuilder app,string tag)
        {
            var route = app.MapGroup("/api/Employee");

            route.MapGet("/", async (IEmployeeService service) =>
            {
                BaseResponse<List<EmployeeResponse>> result = await service.GetAll();
                return result.ToResult();
            }).WithTags(tag);

            route.MapGet("/{Id:long}", async (IEmployeeService service, long Id) =>
            {
                BaseResponse<EmployeeResponse?> result = await service.GetByID(Id);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/create", async (IEmployeeService service, EmployeeCreateDto dto) =>
            {
                BaseResponse<EmployeeResponse> result = await service.Create(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/search", async (IEmployeeService service, SearchEmployeeDto dto) =>
            {
                BaseResponse<List<EmployeeResponse>> result = await service.Search(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPut("/edit", async (IEmployeeService service, EmployeeUpdateDto dto) =>
            {
                BaseResponse<EmployeeResponse?> result = await service.Update(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapDelete("/delete", async (IEmployeeService service, long id) =>
            {
                BaseResponse result = await service.Delete(id);
                return result.ToResult();

            }).WithTags(tag);

        }
    }
}
