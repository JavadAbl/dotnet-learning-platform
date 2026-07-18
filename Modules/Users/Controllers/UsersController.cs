using Microsoft.AspNetCore.Mvc;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {


        [HttpGet("test")]
        public IActionResult test()
        {
            return Ok(321321);
        }
    }
}