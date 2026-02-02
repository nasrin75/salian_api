using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Permission;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Services
{
    public class PermissionService(ApplicationDbContext _dbContex) : IPermissionService
    {
        public async Task<BaseResponse<PermissionResponse?>> Create(PermissionCreateDto param)
        {
            try
            {
                PermissionEntity permission = new PermissionEntity
                {
                    Name = param.Name,
                };

                var newPermission = _dbContex.Permissions.Add(permission).Entity;
                await _dbContex.SaveChangesAsync();

                PermissionResponse response = new PermissionResponse
                {
                    Id = newPermission.Id,
                    Name = newPermission.Name,
                };

                return new BaseResponse<PermissionResponse>(response);
            }catch (Exception ex)
            {
                return new BaseResponse<PermissionResponse>(null,400,ex.Message);
            }

        }

        public async Task<BaseResponse> Delete(long id)
        {
            var permission = await _dbContex.Permissions
                .FirstOrDefaultAsync(l => l.Id == id);
            if (permission == null) return new BaseResponse<PermissionResponse?>(null, 400, "Permission Not Found");

            permission.DeletedAt = DateTime.UtcNow;

            _dbContex.Permissions.Update(permission);
            await _dbContex.SaveChangesAsync();

            return new BaseResponse<PermissionResponse?>(null, 200, "Permission Successfully Is Deleted");
        }

        public async Task<BaseResponse<List<PermissionResponse>>> GetAll()
        {
            List<PermissionResponse> permissions = await _dbContex.Permissions
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(l => new PermissionResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                })
                .ToListAsync();

            return new BaseResponse<List<PermissionResponse>>(permissions);
        }

        public async Task<BaseResponse<PermissionResponse?>> GetByID(long PermissionID)
        {
            var permission = await _dbContex.Permissions.AsNoTracking()
                .Select(item => new PermissionResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                })
                .FirstOrDefaultAsync(l => l.Id == PermissionID);

            if (permission == null) return new BaseResponse<PermissionResponse?>(null, 400, "Permission Not Found");
            
            return new BaseResponse<PermissionResponse>(permission);
        }

  
        public async Task<BaseResponse<PermissionResponse?>> Update(PermissionUpdateDto param)
        {
            var permission = await _dbContex.Permissions
                .FirstOrDefaultAsync(l => l.Id == param.Id);

            if (permission == null) return new BaseResponse<PermissionResponse?>(null, 400, "Permission Not Found");

            if (param.Name != null && param.Name != permission.Name) permission.Name = param.Name;

            _dbContex.Update(permission);
            await _dbContex.SaveChangesAsync();

            var response = new PermissionResponse
            {
                Id = permission.Id,
                Name = permission.Name,
            };

            return new BaseResponse<PermissionResponse?>(response);
        }
    }
}
