using salian_api.Dtos.Auth;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Response.Auth;

namespace salian_api.Routes
{
    public static class Auth
    {
        public static void MapAuthRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var Route = app.MapGroup("/api/auth");

            Route.MapPost("/login",async (IAuthService service,LoginDto request) =>
            {
                BaseResponse<LoginResponse> result = await service.Login(request);
                return result.ToResult();
            }).WithTags(tag);
        }
    }
}
