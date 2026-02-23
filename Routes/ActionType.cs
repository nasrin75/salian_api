using salian_api.Config.Permissions;
using salian_api.Dtos.ActionType;
using salian_api.Response;
using salian_api.Services.ActionType;

namespace salian_api.Routes
{
    public static class ActionType
    {
        public static void MapActionTypeRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var route = app.MapGroup("/api/actionType").WithTags(tag);

            route
                .MapGet(
                    "/",
                    async (IActionTypeService service) =>
                    {
                        BaseResponse<List<ActionTypeResponse>> result = await service.GetAll();
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.ActionType.GetAll)
                );

            route
                .MapGet(
                    "/{Id:long}",
                    async (IActionTypeService service, long Id) =>
                    {
                        BaseResponse<ActionTypeResponse?> result = await service.GetByID(Id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(
                        Permissions.ActionType.GetAll,
                        Permissions.ActionType.Edit
                    )
                );

            route
                .MapPost(
                    "/create",
                    async (IActionTypeService service, ActionTypeCreateDto dto) =>
                    {
                        BaseResponse<ActionTypeResponse> result = await service.Create(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.ActionType.Create)
                );

            route
                .MapPost(
                    "/search",
                    async (IActionTypeService service, SearchActionTypeDto dto) =>
                    {
                        BaseResponse<List<ActionTypeResponse>> result = await service.Search(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.ActionType.GetAll)
                );

            route
                .MapPut(
                    "/edit",
                    async (IActionTypeService service, ActionTypeUpdateDto dto) =>
                    {
                        BaseResponse<ActionTypeResponse?> result = await service.Update(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.ActionType.Edit)
                );

            route
                .MapDelete(
                    "/delete",
                    async (IActionTypeService service, long id) =>
                    {
                        BaseResponse result = await service.Delete(id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.ActionType.Delete)
                );
        }
    }
}
