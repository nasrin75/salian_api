using salian_api.Dtos.Location;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Routes
{
    public static class Location 
    {
        public static void MapLocationRoutes(this IEndpointRouteBuilder app,string tag)
        {
            var route = app.MapGroup("/api/Location");

            route.MapGet("/", async (ILocationService service) =>
            {
                BaseResponse<List<LocationResponse>> result = await service.GetAll();
                return result.ToResult();
            }).WithTags(tag);

            route.MapGet("/{Id:long}", async (ILocationService service, long Id) =>
            {
                BaseResponse<LocationResponse?> result = await service.GetByID(Id);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/Create", async (ILocationService service, LocationCreateDto dto) =>
            {
                BaseResponse<LocationResponse> result = await service.Create(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/Search", async (ILocationService service, SearchLocationDto dto) =>
            {
                BaseResponse<List<LocationResponse>> result = await service.Search(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPut("/edit", async (ILocationService service, LocationUpdateDto dto) =>
            {
                BaseResponse<LocationResponse?> result = await service.Update(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapDelete("/delete", async (ILocationService service, long id) =>
            {
                BaseResponse result = await service.Delete(id);
                return result.ToResult();

            }).WithTags(tag);

        }
    }
}
