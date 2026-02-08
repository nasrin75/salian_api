using Microsoft.AspNetCore.Mvc;

namespace salian_api.Routes;

public static class ApiRoute
{
    public static void MapApiRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/upload", async ([FromForm] IFormFile file) =>
            {
                if (file == null || file.Length == 0)
                    return Results.BadRequest("No file uploaded");

                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/inventory");

                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(imagesFolder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                return Results.Ok(new { fileName });

            })
            .WithTags("Upload")
            .DisableAntiforgery();
    }
}