using salian_api.Dtos.Role;
using salian_api.Dtos.User;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Routes
{
    public static class Role
    {
        public static void MapRoleRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var route = app.MapGroup("api/role");

            route.MapPost("/Create", async(IRoleService service, CreateDto dto) => {
                BaseResponse<RoleResponse> role = await service.Create(dto);
                return role.ToResult();
            }).WithTags(tag);

            route.MapGet("/{id:long}", async (IRoleService service, long id) =>
            {
                BaseResponse<RoleResponse> role = await service.GetRoleByID(id);
                return  role.ToResult();
            }).WithTags(tag);

            route.MapGet("/", async (IRoleService service) =>
            {
                var role = await service.GetAllRoles();
                return role.ToResult();
            }).WithTags(tag);


            route.MapPut("/edit", async (IRoleService service,UpdateDto dto) =>
            {
                BaseResponse<RoleResponse> role = await service.Update(dto);
                return role.ToResult();
            }).WithTags(tag);

            route.MapDelete("/delete",async (IRoleService service,long id) =>
            {
               BaseResponse result = await service.Delete(id);
                return result.ToResult();

            }).WithTags(tag);


        }
    }
}
