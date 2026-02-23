using System.Data;
using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Role;
using salian_api.Entities;
using salian_api.Infrastructure.Data;
using salian_api.Response;

namespace salian_api.Services.Role
{
    public class RoleService(ApplicationDbContext _dbContext) : IRoleService
    {
        public async Task<BaseResponse<RoleResponse>> Create(RoleCreateDto dto)
        {
            var role = new RoleEntity() { FaName = dto.FaName, EnName = dto.EnName };

            var newRole = _dbContext.Roles.Add(role).Entity;
            await _dbContext.SaveChangesAsync();

            RoleResponse response = new RoleResponse
            {
                Id = newRole.Id,
                FaName = dto.FaName,
                EnName = dto.EnName,
            };

            return new BaseResponse<RoleResponse>(response);
        }

        public async Task<BaseResponse> Delete(long id)
        {
            RoleEntity? role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
                return new BaseResponse<RoleResponse?>(null, 400, "ROLE_NOT_FOUND");

            role.DeletedAt = DateTime.UtcNow;
            _dbContext.Roles.Update(role);
            await _dbContext.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse<List<RoleResponse>>> GetAllRoles()
        {
            List<RoleResponse> roles = await _dbContext
                .Roles.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(r => new RoleResponse
                {
                    Id = r.Id,
                    FaName = r.FaName,
                    EnName = r.EnName,
                })
                .ToListAsync();

            return new BaseResponse<List<RoleResponse>>(roles);
        }

        public async Task<BaseResponse<RoleResponse?>> GetRoleByID(long RoleID)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == RoleID);
            if (role == null)
                return new BaseResponse<RoleResponse?>(null, 400, "ROLE_NOT_FOUND");

            RoleResponse response = new RoleResponse()
            {
                Id = role.Id,
                FaName = role.FaName,
                EnName = role.EnName,
            };

            return new BaseResponse<RoleResponse>(response);
        }

        public async Task<BaseResponse<RoleResponse?>> Update(RoleUpdateDto dto)
        {
            RoleEntity role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == dto.Id);

            if (role == null)
                return new BaseResponse<RoleResponse?>(null, 400, "ROLE_NOT_FOUND");
            if (dto.FaName != null)
            {
                role.FaName = dto.FaName;
            }
            if (dto.EnName != null)
            {
                role.EnName = dto.EnName;
            }

            _dbContext.Roles.Update(role);
            await _dbContext.SaveChangesAsync();

            RoleResponse response = new RoleResponse()
            {
                Id = role.Id,
                FaName = role.FaName,
                EnName = role.EnName,
            };

            return new BaseResponse<RoleResponse>(response);
        }

        public async Task<BaseResponse> AssignPermission(AssignRolePermissionDto request)
        {
            RoleEntity role = _dbContext
                .Roles.Include(r => r.Permissions)
                .Where(role => role.Id == request.RoleId)
                .FirstOrDefault();
            if (role == null)
                return new BaseResponse<RoleEntity>(null, 400, "ROLE_NOT_FOUND");

            // delete before data
            role.Permissions.Clear();

            var permissions = _dbContext
                .Permissions.Where(p => request.PermissionIds.Contains(p.Id))
                .ToList();

            foreach (var permission in permissions)
            {
                role.Permissions.Add(permission);
            }

            await _dbContext.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse<List<PermissionResponse>>> GetRolePermissions(long roleID)
        {
            var role = await _dbContext
                .Roles.Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleID);
            if (role == null)
                return new BaseResponse<List<PermissionResponse>>(null, 400, "ROLE_NOT_FOUND");

            var permissions = role
                .Permissions.Select(p => new PermissionResponse { Id = p.Id, Name = p.Name })
                .ToList();

            return new BaseResponse<List<PermissionResponse>>(permissions);
        }
    }
}
