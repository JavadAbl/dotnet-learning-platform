using Contracts.Dto.Request;
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
        public async Task<IActionResult> UserGetMany([FromQuery] GetManyQuery? query)
        {
            return Ok(await userService.UserGetMany(query));
        }


        [HttpPost()]
        [ValidateDto]
        public IActionResult UserCreate([FromBody] UserCreateDto dto)
        {
            return Ok(321321);
        }
    }
}