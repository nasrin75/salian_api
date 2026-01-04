using salian_api.Dtos.User;
using salian_api.Interface;

namespace salian_api.Routes
{
    public static class User
    {
        public static void MapUserRoutes(this IEndpointRouteBuilder app,string tag)
        {
            var route = app.MapGroup("/api/user");

            route.MapGet("/", async (IUserService service) =>
            {
                List<UserResponse> user = await service.GetAllUsers();
                return user != null ? Results.Ok(user) : Results.NotFound();
            }).WithTags(tag);

            route.MapPost("/{id:long}", async (IUserService service,long id) =>
            {
                var user = await service.GetUserByID(id);
                return  user != null ? Results.Ok(user) : Results.NotFound();
            }).WithTags(tag);

            route.MapPost("/create", async (IUserService service, UserCreateDto dto) =>
            {
                var user = await service.Create(dto);
                return Results.Ok(user);
            }).WithTags(tag);

            route.MapPut("/edit", async (IUserService service, UserUpdateDto dto) =>
            {
                var user = await service.Update(dto);
                return user != null ? Results.Ok(user) : Results.NotFound();
                
            }).WithTags(tag);

            route.MapDelete("/delete", async (IUserService service, long userID) =>
            {
                await service.Delete(userID);
                return Results.Ok();
            }).WithTags(tag);

        }
    }
}
