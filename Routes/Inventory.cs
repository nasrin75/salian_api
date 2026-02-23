using salian_api.Config.Permissions;
using salian_api.Dtos.Inventory;
using salian_api.Response;
using salian_api.Response.Inventory;
using salian_api.Services.Inventory;

namespace salian_api.Routes
{
    public static class Inventory
    {
        public static void MapInventoryRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var route = app.MapGroup("/api/inventory").WithTags(tag);

            route
                .MapGet(
                    "/",
                    async (IInventoryService service, string equipment = "ALL") =>
                    {
                        BaseResponse<List<InventoryListResponse>> result = await service.GetAll(
                            equipment
                        );
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Inventory.GetAll)
                );

            route
                .MapGet(
                    "/{Id:long}",
                    async (IInventoryService service, long Id) =>
                    {
                        BaseResponse<InventoryResponse?> result = await service.GetByID(Id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(
                        Permissions.Inventory.GetAll,
                        Permissions.Inventory.Edit
                    )
                );

            route
                .MapPost(
                    "/create",
                    async (IInventoryService service, InventoryCreateDto param) =>
                    {
                        BaseResponse<InventoryResponse> result = await service.Create(param);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Inventory.Create)
                );

            route
                .MapPost(
                    "/search",
                    async (IInventoryService service, SearchInventoryDto dto) =>
                    {
                        BaseResponse<List<InventoryListResponse>> result = await service.Search(
                            dto
                        );
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Inventory.GetAll)
                );

            route
                .MapPut(
                    "/edit",
                    async (IInventoryService service, InventoryUpdateDto dto) =>
                    {
                        BaseResponse<InventoryResponse?> result = await service.Update(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.Inventory.Edit));

            route
                .MapDelete(
                    "/delete",
                    async (IInventoryService service, long id) =>
                    {
                        BaseResponse result = await service.Delete(id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Inventory.Delete)
                );
        }
    }
}
