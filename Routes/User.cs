using salian_api.Config.Permissions;
using salian_api.Dtos.User;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Response.User;

namespace salian_api.Routes
{
    public static class User
    {
        public static void MapUserRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var route = app.MapGroup("/api/user").WithTags(tag);

            route
                .MapGet(
                    "/",
                    async (IUserService service) =>
                    {
                        BaseResponse<List<UserListResponse>> user = await service.GetAllUsers();
                        return user.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.User.GetAll));

            route
                .MapGet(
                    "/{id:long}",
                    async (IUserService service, long id) =>
                    {
                        BaseResponse<UserResponse?> result = await service.GetUserByID(id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.User.Edit, Permissions.User.GetAll)
                );

            route
                .MapPost(
                    "/create",
                    async (IUserService service, UserCreateDto dto) =>
                    {
                        BaseResponse<UserResponse> user = await service.Create(dto);
                        return user.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.User.Create));

            route
                .MapPut(
                    "/edit",
                    async (IUserService service, UserUpdateDto dto) =>
                    {
                        BaseResponse<UserResponse?> user = await service.Update(dto);
                        return user.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.User.Edit));

            route
                .MapDelete(
                    "/delete",
                    async (IUserService service, long id) =>
                    {
                        BaseResponse result = await service.Delete(id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.User.Delete));

            route
                .MapPost(
                    "/search",
                    async (IUserService service, UserSearchDto dto) =>
                    {
                        BaseResponse<List<UserListResponse>> result = await service.Search(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.User.GetAll));

            route
                .MapGet(
                    "/permissions",
                    async (IUserService service) =>
                    {
                        BaseResponse result = await service.UserPermissions();
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.User.AddPermission)
                );
        }
    }
}
