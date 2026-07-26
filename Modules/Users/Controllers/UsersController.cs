using Contracts.Filters;
using Microsoft.AspNetCore.Mvc;
using Users.Contracts.Services;
using Users.Dto.Request;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    internal class UsersController(IUserService userService) : ControllerBase
    {

        [HttpGet()]
        internal IActionResult UserGetMany()
        {
            return Ok([]);
        }


        [HttpPost()]
        [ValidateDto]
        internal IActionResult UserCreate([FromBody] UserCreateDto dto)
        {
            return Ok(321321);
        }
    }
}