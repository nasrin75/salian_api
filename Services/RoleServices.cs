using AutoMapper;
using Microsoft.EntityFrameworkCore;
using salian_api.Contracts.Role;
using salian_api.Interface;
using salian_api.Models;

namespace salian_api.Services
{
    public class RoleServices : IRoleServices
    {
        public readonly ApplicationDbContext _context;
        public readonly ILogger<RoleServices> _logger;
        public readonly IMapper _mapper;

        public RoleServices(ApplicationDbContext context, ILogger<RoleServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateRoleAsync(CreateRequest request)
        {
            /*throw new NotImplementedException();*/
            try
            {
                var role = _mapper.Map<Role>(request);
                _context.Add(role);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the Todo item.");
                throw new Exception("An error occurred while creating the Todo item.");
            }
        }

        public Task DeleteRoleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            if(roles == null)
            {
                throw new Exception(" No Roles items found");
            }

            return roles;
        }

        public Task<Role> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetRoles()
        {
            throw new NotImplementedException();
        }

        public Task UpdateRoleAsync(int id, UpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
