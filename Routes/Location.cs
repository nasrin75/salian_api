using salian_api.Config.Permissions;
using salian_api.Dtos.Location;
using salian_api.Response;
using salian_api.Services.Location;

namespace salian_api.Routes
{
    public static class Location
    {
        public static void MapLocationRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var route = app.MapGroup("/api/location").WithTags(tag);

            route
                .MapGet(
                    "/{status?}",
                    async (ILocationService service, string? status = "ALL") =>
                    {
                        BaseResponse<List<LocationResponse>> result = await service.GetAll(status);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Location.GetAll)
                );

            route
                .MapGet(
                    "/{Id:long}",
                    async (ILocationService service, long Id) =>
                    {
                        BaseResponse<LocationResponse?> result = await service.GetByID(Id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(
                        Permissions.Location.GetAll,
                        Permissions.Location.Edit
                    )
                );

            route
                .MapPost(
                    "/create",
                    async (ILocationService service, LocationCreateDto dto) =>
                    {
                        BaseResponse<LocationResponse> result = await service.Create(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Location.Create)
                );

            route
                .MapPost(
                    "/search",
                    async (ILocationService service, SearchLocationDto dto) =>
                    {
                        BaseResponse<List<LocationResponse>> result = await service.Search(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Location.GetAll)
                );

            route
                .MapPut(
                    "/edit",
                    async (ILocationService service, LocationUpdateDto dto) =>
                    {
                        BaseResponse<LocationResponse?> result = await service.Update(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.Location.Edit));

            route
                .MapDelete(
                    "/delete",
                    async (ILocationService service, long id) =>
                    {
                        BaseResponse result = await service.Delete(id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Location.Delete)
                );
        }
    }
}
