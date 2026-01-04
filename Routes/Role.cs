using salian_api.Dtos.Role;
using salian_api.Interface;

namespace salian_api.Routes
{
    public static class Role
    {
        public static void MapRoleRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var route = app.MapGroup("api/role");

            route.MapPost("/Create", async(IRoleService service, RoleCreateDto dto) => {
                var role = await service.Create(dto);
                return Results.Ok(role);
            }).WithTags(tag);

            route.MapGet("/{id:long}", async (IRoleService service, long id) =>
            {
                var role = await service.GetRoleByID(id);
                Console.WriteLine(role);
                return  role != null ? Results.Ok(role) : Results.NotFound();
            }).WithTags(tag);

            route.MapGet("/", async (IRoleService service) =>
            {
                var role = await service.GetAllRoles();
                return role != null ? Results.Ok(role) : Results.NotFound();
            }).WithTags(tag);


            route.MapPut("/edit", async (IRoleService service,RoleUpdateDto dto) =>
            {
                var role = await service.Update(dto);
                return role != null ? Results.Ok(role) : Results.NotFound();
            }).WithTags(tag);

            route.MapDelete("/delete",async (IRoleService service,long id) =>
            {
                await service.Delete(id);
                return Results.Ok();

            }).WithTags(tag);


        }
    }
}
