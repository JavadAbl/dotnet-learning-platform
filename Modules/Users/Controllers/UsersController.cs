using Shared.Dto.Request;
using Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Users.Shared.Services;
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


        [HttpGet("{:id}")]
        public async Task<IActionResult> UserGetById([FromRoute] int id)
        {
            return Ok(await userService.UserGetById(id));
        }


        [HttpPost()]
        // [ValidateDto]
        public async Task<IActionResult> UserCreate([FromBody] UserCreateDto dto)
        {
            var userId = await userService.UserCreate(dto);
            return Ok(userId);
        }
    }
}