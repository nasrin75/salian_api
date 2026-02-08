
using salian_api.Dtos.Profile;
using salian_api.Interface;

namespace salian_api.Response
{
    public static class Profile 
    {
        public static void MapProfileRoutes(this IEndpointRouteBuilder app,string tag)
        {
            var route = app.MapGroup("/api/profile");

            route.MapPut("/setting/edit", async (IProfileService service, ProfileUpdateDto dto) =>
            {
                BaseResponse<ProfileResponse?> result = await service.Update(dto);
                return result.ToResult();
            }).WithTags(tag);

            route.MapGet("/{id:long}", async (IProfileService service,long Id) =>
            {
                BaseResponse<ProfileResponse?> result = await service.GetByID(Id);
                return result.ToResult();
            }).WithTags(tag);
        }
    }
}
