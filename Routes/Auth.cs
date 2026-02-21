using salian_api.Dtos.Auth;
using salian_api.Response;
using salian_api.Response.Auth;
using salian_api.Services.Auth;

namespace salian_api.Routes
{
    public static class Auth
    {
        public static void MapAuthRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var Route = app.MapGroup("/api/auth");

            Route
                .MapPost(
                    "/login",
                    async (IAuthService service, LoginDto request) =>
                    {
                        BaseResponse<LoginResponse> result = await service.Login(request);
                        return result.ToResult();
                    }
                )
                .WithTags(tag);

            Route
                .MapPost(
                    "ResetPassword",
                    async (IAuthService service, string email) =>
                    {
                        BaseResponse result = await service.SendResetPasswordEmail(email);
                        return result.ToResult();
                    }
                )
                .WithTags(tag);
        }
    }
}
