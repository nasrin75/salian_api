using salian_api.Response;
using salian_api.Services.Me;

namespace salian_api.Routes
{
    public static class MyRoutes
    {
        public static void MapMyRoutes(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                    "/api/myPermissions",
                    async (IMeService service) =>
                    {
                        BaseResponse result = await service.MyPermissions();
                        return result.ToResult();
                    }
                )
                .WithTags("User")
                .RequireAuthorization();
        }
    }
}
