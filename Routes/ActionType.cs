using salian_api.Dtos.ActionType;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Routes
{
    public static class ActionType 
    {
        public static void MapActionTypeRoutes(this IEndpointRouteBuilder app,string tag)
        {
            var route = app.MapGroup("/api/actionType");

            route.MapGet("/", async (IActionTypeService service) =>
            {
                BaseResponse<List<ActionTypeResponse>> result = await service.GetAll();
                return result.ToResult();
            }).WithTags(tag);

            route.MapGet("/{Id:long}", async (IActionTypeService service, long Id) =>
            {
                BaseResponse<ActionTypeResponse?> result = await service.GetByID(Id);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/create", async (IActionTypeService service, ActionTypeCreateDto dto) =>
            {
                BaseResponse<ActionTypeResponse> result = await service.Create(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/search", async (IActionTypeService service, SearchActionTypeDto dto) =>
            {
                BaseResponse<List<ActionTypeResponse>> result = await service.Search(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPut("/edit", async (IActionTypeService service, ActionTypeUpdateDto dto) =>
            {
                BaseResponse<ActionTypeResponse?> result = await service.Update(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapDelete("/delete", async (IActionTypeService service, long id) =>
            {
                BaseResponse result = await service.Delete(id);
                return result.ToResult();

            }).WithTags(tag);

        }
    }
}
