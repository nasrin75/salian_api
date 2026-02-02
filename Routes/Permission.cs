using salian_api.Dtos.Permission;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Routes
{
    public static class Permission 
    {
        public static void MapPermissionRoutes(this IEndpointRouteBuilder app,string tag)
        {
            var route = app.MapGroup("/api/permission");

            route.MapGet("/", async (IPermissionService service) =>
            {
                BaseResponse<List<PermissionResponse>> result = await service.GetAll();
                return result.ToResult();
            }).WithTags(tag);

            route.MapGet("/{Id:long}", async (IPermissionService service, long Id) =>
            {
                BaseResponse<PermissionResponse?> result = await service.GetByID(Id);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/create", async (IPermissionService service, PermissionCreateDto dto) =>
            {
                BaseResponse<PermissionResponse> result = await service.Create(dto);
                return result.ToResult();
            }).WithTags(tag);



            route.MapPut("/edit", async (IPermissionService service, PermissionUpdateDto dto) =>
            {
                BaseResponse<PermissionResponse?> result = await service.Update(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapDelete("/delete", async (IPermissionService service, long id) =>
            {
                BaseResponse result = await service.Delete(id);
                return result.ToResult();

            }).WithTags(tag);

        }
    }
}
