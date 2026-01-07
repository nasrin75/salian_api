using salian_api.Dtos.Feature;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Routes
{
    public static class Feature 
    {
        public static void MapFeatureRoutes(this IEndpointRouteBuilder app,string tag)
        {
            var route = app.MapGroup("/api/feature");

            route.MapGet("/", async (IFeatureService service) =>
            {
                BaseResponse<List<FeatureResponse>> result = await service.GetAll();
                return result.ToResult();
            }).WithTags(tag);

            route.MapGet("/{Id:long}", async (IFeatureService service, long Id) =>
            {
                BaseResponse<FeatureResponse?> result = await service.GetByID(Id);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/create", async (IFeatureService service, FeatureCreateDto dto) =>
            {
                BaseResponse<FeatureResponse> result = await service.Create(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/search", async (IFeatureService service, SearchFeatureDto dto) =>
            {
                BaseResponse<List<FeatureResponse>> result = await service.Search(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPut("/edit", async (IFeatureService service, FeatureUpdateDto dto) =>
            {
                BaseResponse<FeatureResponse?> result = await service.Update(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapDelete("/delete", async (IFeatureService service, long id) =>
            {
                BaseResponse result = await service.Delete(id);
                return result.ToResult();

            }).WithTags(tag);

        }
    }
}
