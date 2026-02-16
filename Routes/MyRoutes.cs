using salian_api.Interface;
using salian_api.Response;

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
