using salian_api.Dtos.Equipment;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Response.Equipment;

namespace salian_api.Routes
{
    public static class Equipment
    {
        public static void MapEquipmentRoutes(this IEndpointRouteBuilder app,string tag)
        {
           var route = app.MapGroup("/api/equipment");

            route.MapGet("/", async (IEquipmentService service) =>
            {
                BaseResponse<List<EquipmentListResponse>> result = await service.GetAll();
                return result.ToResult();
            }).WithTags(tag);

            route.MapGet("/{Id:long}", async (IEquipmentService service, long Id) =>
            {
                BaseResponse<EquipmentResponse?> result = await service.GetByID(Id);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/create", async (IEquipmentService service, EquipmentCreateDto dto) =>
            {
                BaseResponse<EquipmentResponse> result = await service.Create(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/search", async (IEquipmentService service, SearchEquipmentDto dto) =>
            {
                BaseResponse<List<EquipmentResponse>> result = await service.Search(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPut("/edit", async (IEquipmentService service, EquipmentUpdateDto dto) =>
            {
                BaseResponse<EquipmentResponse?> result = await service.Update(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapDelete("/delete", async (IEquipmentService service, long id) =>
            {
                BaseResponse result = await service.Delete(id);
                return result.ToResult();

            }).WithTags(tag);

            route.MapGet("/features/{Id:long}", async (IEquipmentService service,long id) =>
            {
                var result = await service.GetFeaturesByEquipmentID(id);
                return result.ToResult();
            }).WithTags(tag);
        }
    }
}
