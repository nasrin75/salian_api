using salian_api.Dtos.User;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Routes
{
    public static class User
    {
        public static void MapUserRoutes(this IEndpointRouteBuilder app,string tag)
        {
            var route = app.MapGroup("/api/user");

            route.MapGet("/", async (IUserService service) =>
            {
               BaseResponse<List<UserResponse>> user = await service.GetAllUsers();
                return user.ToResult();
            }).WithTags(tag);

            route.MapGet("/{id:long}", async (IUserService service,long id) =>
            {
                BaseResponse<UserResponse?> result = await service.GetUserByID(id);
                return result.ToResult();
            }).WithTags(tag);

            route.MapPost("/create", async (IUserService service, LocationCreateDto dto) =>
            {
                BaseResponse<UserResponse> user = await service.Create(dto);
                return user.ToResult();
            }).WithTags(tag);

            route.MapPut("/edit", async (IUserService service, UpdateDto dto) =>
            {
                BaseResponse<UserResponse?> user = await service.Update(dto);
                return user.ToResult();
                
            }).WithTags(tag);

            route.MapDelete("/delete", async (IUserService service, long userID) =>
            {
               BaseResponse result = await service.Delete(userID);
                return result.ToResult();
            }).WithTags(tag);

        }
    }
}
