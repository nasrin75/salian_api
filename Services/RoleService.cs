using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Role;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Routes;
using System.Data;

namespace salian_api.Services
{
    public class RoleService(ApplicationDbContext dbContext) : IRoleService
    {
        public async Task<RoleResponse> Create(RoleCreateDto dto)
        {
            var role = new RoleEntity()
            {
                FaName = dto.FaName,
                EnName = dto.EnName,
            };

            var newRole = dbContext.Roles.Add(role).Entity;
            await dbContext.SaveChangesAsync();

            return new RoleResponse
            {
                Id = newRole.Id,
                FaName = dto.FaName,
                EnName = dto.EnName,
            };
        }

        public async Task Delete(long id)
        {
            RoleEntity? role = await dbContext.Roles.FindAsync(id);
            if (role == null) return;

            role.IsDeleted = true;
            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<RoleResponse>> GetAllRoles()
        {
           List<RoleResponse> roles = await dbContext.Roles
                .Select(r => new RoleResponse
                {
                    Id = r.Id,
                    FaName = r.FaName,
                    EnName = r.EnName
                }).ToListAsync();

            return new List<RoleResponse>(roles);
            
        }

        public async Task<RoleResponse?> GetRoleByID(long RoleID)
        {
            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == RoleID);
            if (role == null) return null;

           return new RoleResponse()
            {
                Id = role.Id,
                FaName = role.FaName,
                EnName = role.EnName
            };
        }

        public async Task<RoleResponse?> Update(RoleUpdateDto dto)
        {
            RoleEntity role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == dto.Id);

            if (role == null) return null;
            if(dto.FaName != null) { role.FaName = dto.FaName; }
            if(dto.EnName != null) { role.EnName = dto.EnName; }

            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();

            return new RoleResponse()
            {
                Id = role.Id,
                FaName = role.FaName,
                EnName = role.EnName,
            };
        }
    }
}
