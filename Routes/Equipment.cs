using salian_api.Config.Permissions;
using salian_api.Dtos.Equipment;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Response.Equipment;

namespace salian_api.Routes
{
    public static class Equipment
    {
        public static void MapEquipmentRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var route = app.MapGroup("/api/equipment").WithTags(tag);

            route
                .MapGet(
                    "/",
                    async (IEquipmentService service) =>
                    {
                        BaseResponse<List<EquipmentListResponse>> result = await service.GetAll();
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Equipment.GetAll)
                );

            route
                .MapGet(
                    "/{Id:long}",
                    async (IEquipmentService service, long Id) =>
                    {
                        BaseResponse<EquipmentResponse?> result = await service.GetByID(Id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(
                        Permissions.Equipment.GetAll,
                        Permissions.Equipment.Edit
                    )
                );

            route
                .MapPost(
                    "/create",
                    async (IEquipmentService service, EquipmentCreateDto dto) =>
                    {
                        BaseResponse<EquipmentResponse> result = await service.Create(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Equipment.Create)
                );

            route
                .MapPost(
                    "/search",
                    async (IEquipmentService service, SearchEquipmentDto dto) =>
                    {
                        BaseResponse<List<EquipmentResponse>> result = await service.Search(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Equipment.GetAll)
                );

            route
                .MapPut(
                    "/edit",
                    async (IEquipmentService service, EquipmentUpdateDto dto) =>
                    {
                        BaseResponse<EquipmentResponse?> result = await service.Update(dto);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(new PermissionAuthorizeAttribute(Permissions.Equipment.Edit));

            route
                .MapDelete(
                    "/delete",
                    async (IEquipmentService service, long id) =>
                    {
                        BaseResponse result = await service.Delete(id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Equipment.Delete)
                );

            route
                .MapGet(
                    "/features/{Id:long}",
                    async (IEquipmentService service, long id) =>
                    {
                        var result = await service.GetFeaturesByEquipmentID(id);
                        return result.ToResult();
                    }
                )
                .RequireAuthorization(
                    new PermissionAuthorizeAttribute(Permissions.Equipment.GetAll)
                );

            route
                .MapGet(
                    "/inventory/subMenu",
                    async (IEquipmentService service) =>
                    {
                        var result = await service.GetInventorySubMenu();
                        return result.ToResult();
                    }
                )
                .RequireAuthorization();
        }
    }
}
