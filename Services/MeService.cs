using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Services
{
    public class MeService(
        IHttpContextAccessor _httpContextAccessor,
        ApplicationDbContext _dbContext
    ) : IMeService
    {
        public async Task<BaseResponse<List<PermissionResponse>>> MyPermissions()
        {
            var userID = _httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;
            if (userID == null)
                return new BaseResponse<List<PermissionResponse>>(null, 400, "ACCESS_DENY");

            UserEntity user = await _dbContext
                .Users.Include("Permissions")
                .Include(u => u.Role)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(x => x.Id.ToString() == userID);
            if (user == null)
                return new BaseResponse<List<PermissionResponse>>(null, 400, "USER_NOT_FOUND");

            var userPermissions = user
                .Permissions.Select(p => new PermissionResponse { Id = p.Id, Name = p.Name })
                .ToList();
            var rolePermissions = user.Role.Permissions.Select(p => new PermissionResponse
            {
                Id = p.Id,
                Name = p.Name,
                Title = p.Title,
                Category = p.Category,
            })
              .ToList();
            var allPermissions = userPermissions.Union(rolePermissions).ToList();
            return new BaseResponse<List<PermissionResponse>>(allPermissions);
        }
    }
}
