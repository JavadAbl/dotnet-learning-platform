using Microsoft.AspNetCore.Mvc;
using Shared.Dto.Request;
using Users.Dto.Request;
using Users.Shared.Services;


namespace Users.Controllers;

[Route("api/[controller]")]
[ApiController]
internal class UsersController(IUserService userService) : ControllerBase
{

    [HttpGet()]
    public async Task<IActionResult> UserGetMany([FromQuery] GetManyQuery? query)
    {
        return Ok(await userService.UserGetDtoMany(query));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> UserGetById([FromRoute] int id)
    {
        return Ok(await userService.UserGetDtoById(id));
    }


    [HttpPost()]
    public async Task<IActionResult> UserCreate([FromBody] UserCreateDto payload)
    {
        var userId = await userService.UserCreate(payload);
        return Ok(userId);
    }


    [HttpPatch("{id}")]
    public async Task<IActionResult> UserUpdate([FromRoute] int id, [FromBody] UserUpdateDto payload)
    {
        await userService.UserUpdate(id, payload);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> UserDelete([FromRoute] int id)
    {
        await userService.UserDelete(id);
        return NoContent();
    }
}
