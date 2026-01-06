using Azure;
using Humanizer;
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

                var newEmployee = _dbContex.Employees.Add(employee);
                await _dbContex.SaveChangesAsync();

                EmployeeResponse response = new EmployeeResponse
                {
                    Id = newEmployee.Entity.Id,
                    Name = newEmployee.Entity.Name,
                    Email = newEmployee.Entity.Email,
                    /*Location = newEmployee.Entity.Location.Abbreviation*/ //TODO
                };

                return new BaseResponse<EmployeeResponse>(response);
            }catch (Exception ex)
            {
                return new BaseResponse<EmployeeResponse>(null,400,ex.Message);
            }

        }

        public async Task<BaseResponse> Delete(long id)
        {
            throw new NotImplementedException();
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
                    /*Location = _dbContex.Locations.Where(x => x.Id == l.LocationID).Select(x "Abbreviation").Firs*/ //TODO
                })
                .ToListAsync();

            return new BaseResponse<List<EmployeeResponse>>(employees);
        }

        public async Task<BaseResponse<EmployeeResponse?>> GetByID(long EmployeeID)
        {
            var employee = await _dbContex.Employees.FirstOrDefaultAsync(l => l.Id == EmployeeID);
            if (employee == null) return new BaseResponse<EmployeeResponse?>(null, 400, "Employee Not Found");

            var response = new EmployeeResponse
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
               /* Location = employee.IsShow,*/ //TODO
            };

            return new BaseResponse<EmployeeResponse>(response);
        }

        public async Task<BaseResponse<List<EmployeeResponse>>> Search(SearchEmployeeDto param)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<EmployeeResponse?>> Update(EmployeeUpdateDto param)
        {
            var employee = await _dbContex.Employees
                .FirstOrDefaultAsync(l => l.Id == param.Id);

            if (employee == null) return new BaseResponse<EmployeeResponse?>(null, 400, "Employee Not Found");

            if (param.Name != null && param.Name != employee.Name) employee.Name = param.Name;
            if (param.Email != null && param.Email != employee.Email) employee.Email = param.Email;
            if (param.LocationID != null && param.LocationID != employee.LocationID) employee.LocationID = param.LocationID.Value;

            _dbContex.Employees.Update(employee);
            await _dbContex.SaveChangesAsync();

            EmployeeResponse response = new EmployeeResponse
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
               // Location = employee.Location
            };

            return new BaseResponse<EmployeeResponse?>(response);
        }
    }
}
