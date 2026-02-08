using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace salian_api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public IActionResult CreateToken()
        {

            return Ok();
        }
    }
}
