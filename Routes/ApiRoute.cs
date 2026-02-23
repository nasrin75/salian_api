using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using salian_api.Dtos.Email;
using salian_api.Dtos.Otp;
using salian_api.Response;
using salian_api.Response.Otp;
using salian_api.Services.Auth;
using salian_api.Services.Mail;
using salian_api.Services.Sms;

namespace salian_api.Routes;

public static class ApiRoute
{
    public static void MapApiRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "api/upload",
                async ([FromForm] IFormFile file) =>
                {
                    if (file == null || file.Length == 0)
                        return Results.BadRequest("NO_FILE_UPDATED");

                    var imagesFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/images/inventory"
                    );

                    if (!Directory.Exists(imagesFolder))
                        Directory.CreateDirectory(imagesFolder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(imagesFolder, fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);

                    return Results.Ok(new { fileName });
                }
            )
            .WithTags("Upload")
            .DisableAntiforgery()
            .RequireAuthorization();

        app.MapPost(
                "/sendOtp",
                async (ISmsService service, SendOtpDto request) =>
                {
                    var result = await service.SendOtp(request);
                    return result.ToResult();
                }
            )
            .WithTags("Otp");

        app.MapPost(
                "/sendEmail",
                async (IMailService service, SendMailDto request) =>
                {
                    await service.SendEmail(request);
                }
            )
            .WithTags("SendEmail");
    }
}
