using salian_api.Config.Permissions;
using salian_api.Dtos.Role;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Routes
{
    public static class Role
    {
        public static void MapRoleRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var route = app.MapGroup("api/role").WithTags(tag);

            route
                .MapGet(
                    "/",
                    async (IRoleService service) =>
                    {
                        var role = await service.GetAllRoles();
                        return role.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(
                        Permissions.Role.GetAll,
                        Permissions.User.Create,
                        Permissions.User.Edit
                    )
                );

            route
                .MapGet(
                    "/{id:long}",
                    async (IRoleService service, long id) =>
                    {
                        BaseResponse<RoleResponse> role = await service.GetRoleByID(id);
                        return role.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Role.GetAll, Permissions.Role.Edit)
                );

            route
                .MapPost(
                    "/create",
                    async (IRoleService service, RoleCreateDto dto) =>
                    {
                        BaseResponse<RoleResponse> role = await service.Create(dto);
                        return role.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.Role.Create));

            route
                .MapPut(
                    "/edit",
                    async (IRoleService service, RoleUpdateDto dto) =>
                    {
                        BaseResponse<RoleResponse> role = await service.Update(dto);
                        return role.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.Role.Edit));

            route
                .MapDelete(
                    "/delete",
                    async (IRoleService service, long id) =>
                    {
                        BaseResponse result = await service.Delete(id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.Role.Delete));

            route
                .MapPost(
                    "/permission/add",
                    async (IRoleService service, AssignRolePermissionDto request) =>
                    {
                        BaseResponse result = await service.AssignPermission(request);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Role.AddPermission)
                );
        }
    }
}
