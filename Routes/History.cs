using salian_api.Config.Permissions;
using salian_api.Dtos.History;
using salian_api.Services.History;

namespace salian_api.Routes
{
    public static class History
    {
        public static void MapHistoryRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var route = app.MapGroup("api").WithTags(tag);

            route
                .MapPost(
                    "/history",
                    async (IHistoryService service, HistorySearchParamsDto request) =>
                    {
                        var histories = await service.GetAllHistory(request);
                        return histories.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.History.GetAll));
            route
                .MapPost(
                    "/{entity}/{entityId}/history",
                    async (IHistoryService service, string entity, long entityId) =>
                    {
                        var histories = await service.GetByEntity(entity, entityId);
                        return histories.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.User.History));
            route
                .MapGet(
                    "history/{id}",
                    async (IHistoryService service, long id) =>
                    {
                        var histories = await service.GetHistory(id);
                        return histories.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.User.GetAll));
            route
                .MapDelete(
                    "/history/delete",
                    async (IHistoryService service, long id) =>
                    {
                        var result = await service.Delete(id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.History.Delete));
        }
    }
}
