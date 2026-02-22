using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using salian_api.Dtos.Auth;
using salian_api.Response;
using salian_api.Services.Password;
using static salian_api.Config.Permissions.Permissions;

namespace salian_api.Routes
{
    public static class Password
    {
        public static void MapPasswordRoutes(this IEndpointRouteBuilder app, string tag)
        {
            var Route = app.MapGroup("/api");

            Route
                .MapPost(
                    "/resetPassword",
                    async (IPasswordService service, string email) =>
                    {
                        BaseResponse result = await service.SendResetPasswordByEmail(email);
                        return result.ToResult();
                    }
            )
            .WithTags(tag);

            Route.MapPost("verifyResetPassword", async (IPasswordService service,VerifyResetPasswordDto request) =>
            {
                BaseResponse result = await service.VerifyResetPasswordByEmail(request);
                return result.ToResult();
            });
        }
    }
}
