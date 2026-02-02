using Microsoft.EntityFrameworkCore;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;
using System.Data;

namespace salian_api.Services
{
    public class RoleService(ApplicationDbContext dbContext) : IRoleService
    {
        public async Task<BaseResponse<RoleResponse>> Create(Dtos.Role.RoleCreateDto dto)
        {
            var role = new RoleEntity()
            {
                FaName = dto.FaName,
                EnName = dto.EnName,
            };

            var newRole = dbContext.Roles.Add(role).Entity;
            await dbContext.SaveChangesAsync();

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
            RoleEntity? role = await dbContext.Roles.FindAsync(id);
            if (role == null) return new BaseResponse<RoleResponse?>(null, 400, "Role Not Found");

            role.DeletedAt = DateTime.UtcNow;
            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse<List<RoleResponse>>> GetAllRoles()
        {
           List<RoleResponse> roles = await dbContext.Roles
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(r => new RoleResponse
                {
                    Id = r.Id,
                    FaName = r.FaName,
                    EnName = r.EnName
                }).ToListAsync();

            return new BaseResponse<List<RoleResponse>>(roles);
            
        }

        public async Task<BaseResponse<RoleResponse?>> GetRoleByID(long RoleID)
        {
            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == RoleID);
            if (role == null) return new BaseResponse<RoleResponse?>(null, 400, "Role Not Found"); ;

            RoleResponse response = new RoleResponse()
            {
                Id = role.Id,
                FaName = role.FaName,
                EnName = role.EnName
            };

            return new BaseResponse<RoleResponse>(response);
        }


        public async Task<BaseResponse<RoleResponse?>> Update(Dtos.Role.RoleUpdateDto dto)
        {
            RoleEntity role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == dto.Id);

            if (role == null) return new BaseResponse<RoleResponse?>(null, 400, "Role Not Found");
            if (dto.FaName != null) { role.FaName = dto.FaName; }
            if(dto.EnName != null) { role.EnName = dto.EnName; }

            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();

            RoleResponse response = new RoleResponse()
            {
                Id = role.Id,
                FaName = role.FaName,
                EnName = role.EnName,
            };

            return new BaseResponse<RoleResponse>(response);
        }
    }
}
