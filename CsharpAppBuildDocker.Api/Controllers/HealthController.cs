using CsharpAppBuildDocker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CsharpAppBuildDocker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController(IHealthService healthService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetStatus()
    {
        var version = healthService.GetVersion();
        return Ok(new { status = healthService.GetStatus(), version=version, commitHash=healthService.GetCommitHash() });
    }
}
