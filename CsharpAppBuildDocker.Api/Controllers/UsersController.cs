using CsharpAppBuildDocker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CsharpAppBuildDocker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet("names")]
    public IActionResult GetUserNames()
    {
        return Ok(userService.GetUserNames());
    }

    [HttpGet("{id:int}/address")]
    public IActionResult GetUserAddress(int id)
    {
        var address = userService.GetUserAddress(id);
        return address is null ? NotFound(new { message = "User not found." }) : Ok(new { id, address });
    }

    [HttpGet("{id:int}/associates")]
    public IActionResult GetAssociates(int id)
    {
        var associates = userService.GetAssociates(id);
        return associates is null ? NotFound(new { message = "User not found." }) : Ok(new { id, associates });
    }
}
