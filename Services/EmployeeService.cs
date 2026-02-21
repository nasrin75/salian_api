using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Employee;
using salian_api.Entities;
using salian_api.Infrastructure.Data;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Services
{
    public class EmployeeService(ApplicationDbContext _dbContex) : IEmployeeService
    {
        public async Task<BaseResponse<EmployeeResponse?>> Create(EmployeeCreateDto param)
        {
            try
            {
                EmployeeEntity employee = new EmployeeEntity
                {
                    Name = param.Name,
                    Email = param.Email,
                    LocationId = param.LocationId,
                };

                var newEmployee = _dbContex.Employees.Add(employee).Entity;
                await _dbContex.SaveChangesAsync();

                var response = await _dbContex
                    .Employees.Where(x => x.Id == newEmployee.Id)
                    .Select(u => new EmployeeResponse
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Location = u.Location.Abbreviation,
                    })
                    .FirstOrDefaultAsync();

                return new BaseResponse<EmployeeResponse>(response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<EmployeeResponse>(null, 400, ex.Message);
            }
        }

        public async Task<BaseResponse> Delete(long id)
        {
            var employee = await _dbContex.Employees.FirstOrDefaultAsync(l => l.Id == id);
            if (employee == null)
                return new BaseResponse<EmployeeResponse?>(null, 400, "EMPLOYEE_NOT_FOUND");

            employee.DeletedAt = DateTime.UtcNow;

            _dbContex.Employees.Update(employee);
            await _dbContex.SaveChangesAsync();

            return new BaseResponse<EmployeeResponse?>(
                null,
                200,
                "Employee Successfully Is Deleted"
            );
        }

        [Authorize(Roles = "User")]
        public async Task<BaseResponse<List<EmployeeResponse>>> GetAll()
        {
            List<EmployeeResponse> employees = await _dbContex
                .Employees.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(l => new EmployeeResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                    Email = l.Email,
                    LocationId = l.LocationId,
                    Location = l.Location.Abbreviation,
                })
                .ToListAsync();

            return new BaseResponse<List<EmployeeResponse>>(employees);
        }

        public async Task<BaseResponse<EmployeeResponse?>> GetByID(long EmployeeID)
        {
            var employee = await _dbContex
                .Employees.AsNoTracking()
                .Select(item => new EmployeeResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    LocationId = item.LocationId,
                    Location = item.Location.Abbreviation,
                })
                .FirstOrDefaultAsync(l => l.Id == EmployeeID);

            if (employee == null)
                return new BaseResponse<EmployeeResponse?>(null, 400, "EMPLOYEE_NOT_FOUND");

            return new BaseResponse<EmployeeResponse>(employee);
        }

        public async Task<BaseResponse<List<EmployeeResponse>>> Search(SearchEmployeeDto param)
        {
            var query = _dbContex.Employees.AsQueryable();
            if (!string.IsNullOrWhiteSpace(param.Name))
                query = query.Where(x => x.Name.Contains(param.Name));
            if (!string.IsNullOrWhiteSpace(param.Email))
                query = query.Where(x => x.Email.Contains(param.Email));
            if (param.LocationId != null)
                query = query.Where(l => l.LocationId == param.LocationId);

            List<EmployeeResponse> employees = await query
                .Select(l => new EmployeeResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                    Email = l.Email,
                    LocationId = l.LocationId,
                    Location = l.Location.Abbreviation,
                })
                .ToListAsync();

            return new BaseResponse<List<EmployeeResponse>>(employees);
        }

        public async Task<BaseResponse<EmployeeResponse?>> Update(EmployeeUpdateDto param)
        {
            var employee = await _dbContex
                .Employees.Include(e => e.Location)
                .FirstOrDefaultAsync(l => l.Id == param.Id);

            if (employee == null)
                return new BaseResponse<EmployeeResponse?>(null, 400, "EMPLOYEE_NOT_FOUND");

            if (param.Name != null && param.Name != employee.Name)
                employee.Name = param.Name;
            if (param.Email != null && param.Email != employee.Email)
                employee.Email = param.Email;
            if (param.LocationId != null && param.LocationId != employee.LocationId)
                employee.LocationId = param.LocationId.Value;

            _dbContex.Update(employee);
            await _dbContex.SaveChangesAsync();

            var response = await _dbContex
                .Employees.Where(x => x.Id == param.Id)
                .Select(u => new EmployeeResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Location = u.Location.Abbreviation,
                })
                .FirstOrDefaultAsync();

            return new BaseResponse<EmployeeResponse?>(response);
        }
    }
}
