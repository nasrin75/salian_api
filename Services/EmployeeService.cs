using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Employee;
using salian_api.Entities;
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
                    LocationID = param.LocationID,
                };

                var newEmployee = _dbContex.Employees.Add(employee).Entity;
                await _dbContex.SaveChangesAsync();

                EmployeeResponse response = new EmployeeResponse
                {
                    Id = newEmployee.Id,
                    Name = newEmployee.Name,
                    Email = newEmployee.Email,
                    Location = newEmployee.Location.Abbreviation
                };

                return new BaseResponse<EmployeeResponse>(response);
            }catch (Exception ex)
            {
                return new BaseResponse<EmployeeResponse>(null,400,ex.Message);
            }

        }

        public async Task<BaseResponse> Delete(long id)
        {
            var employee = await _dbContex.Employees
                .FirstOrDefaultAsync(l => l.Id == id);
            if (employee == null) return new BaseResponse<EmployeeResponse?>(null, 400, "Employee Not Found");

            employee.DeletedAt = DateTime.UtcNow;

            _dbContex.Employees.Update(employee);
            await _dbContex.SaveChangesAsync();

            return new BaseResponse<EmployeeResponse?>(null, 200, "Employee Successfully Is Deleted");
        }

        public async Task<BaseResponse<List<EmployeeResponse>>> GetAll()
        {
            List<EmployeeResponse> employees = await _dbContex.Employees
                .AsNoTracking()
                .Select(l => new EmployeeResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                    Email = l.Email,
                    LocationID = l.LocationID,
                    Location = l.Location.Abbreviation
                })
                .ToListAsync();

            return new BaseResponse<List<EmployeeResponse>>(employees);
        }

        public async Task<BaseResponse<EmployeeResponse?>> GetByID(long EmployeeID)
        {
            var employee = await _dbContex.Employees.AsNoTracking()
                .Select(item => new EmployeeResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    LocationID = item.LocationID,
                    Location = item.Location.Abbreviation
                })
                .FirstOrDefaultAsync(l => l.Id == EmployeeID);

            if (employee == null) return new BaseResponse<EmployeeResponse?>(null, 400, "Employee Not Found");
            
            return new BaseResponse<EmployeeResponse>(employee);
        }

        public async Task<BaseResponse<List<EmployeeResponse>>> Search(SearchEmployeeDto param)
        {
            var query = _dbContex.Employees.AsQueryable();
            if (!string.IsNullOrWhiteSpace(param.Name)) query = query.Where(x => x.Name.Contains(param.Name));
            if (!string.IsNullOrWhiteSpace(param.Email)) query = query.Where(x => x.Email.Contains(param.Email));
            if (param.LocationID != null) query = query.Where(l => l.LocationID == param.LocationID);

            List<EmployeeResponse> employees = await query.Select(l => new EmployeeResponse
            {
                Id = l.Id,
                Name = l.Name,
                Email = l.Email,
                LocationID = l.LocationID,
                Location = l.Location.Abbreviation
            }).ToListAsync();

            return new BaseResponse<List<EmployeeResponse>>(employees);
        }

        public async Task<BaseResponse<EmployeeResponse?>> Update(EmployeeUpdateDto param)
        {
            var employee = await _dbContex.Employees
                .Include(e => e.Location)
                .FirstOrDefaultAsync(l => l.Id == param.Id);

            if (employee == null) return new BaseResponse<EmployeeResponse?>(null, 400, "Employee Not Found");

            if (param.Name != null && param.Name != employee.Name) employee.Name = param.Name;
            if (param.Email != null && param.Email != employee.Email) employee.Email = param.Email;
            if (param.LocationID != null && param.LocationID != employee.LocationID) employee.LocationID = param.LocationID.Value;

            _dbContex.Update(employee);
            await _dbContex.SaveChangesAsync();

            var response = new EmployeeResponse
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                LocationID = employee.LocationID,
                Location = employee.Location.Abbreviation
            };

            return new BaseResponse<EmployeeResponse?>(response);
        }
    }
}
