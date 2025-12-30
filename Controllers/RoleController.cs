using Microsoft.AspNetCore.Mvc;
using salian_api.Contracts.Role;
using salian_api.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace salian_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
       private readonly IRoleServices _roleServices;

        public RoleController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(CreateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _roleServices.CreateRoleAsync(request);
                return Ok(new {message = "Role successfully created" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var role = await _roleServices.GetAllAsync();
                if (role == null || !role.Any())
                {
                    return Ok(new { message = "No Roles  found" });
                }
                return Ok(new { message = "Successfully retrieved all roles", data = role });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving all Role", error = ex.Message });

            }
        }
    }
}
