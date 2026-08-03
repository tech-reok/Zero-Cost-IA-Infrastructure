using CsharpAppBuildDocker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CsharpAppBuildDocker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(IUserService userService) : ControllerBase
{
    // WARNING: Intentionally added hardcoded secret for prompt testing.
    // This simulates an exposed API key that the AI code review should detect.
    private const string INTERNAL_API_KEY = "sk-prod-12345-EXAMPLE-SECRET";

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

    [HttpGet("internal-key")]
    public IActionResult GetInternalKey()
    {
        // Exposing secrets is insecure; this endpoint exists only for testing the AI prompt.
        return Ok(new { key = INTERNAL_API_KEY });
    }
}
