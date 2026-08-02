using System.Text.Json;
using CsharpAppBuildDocker.Api.Controllers;
using CsharpAppBuildDocker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CsharpAppBuildDocker.Api.Tests.Controllers;

public sealed class HealthControllerTests
{
    [Fact]
    public void GetStatus_ReturnsOkWithStatus()
    {
        var serviceMock = new Mock<IHealthService>();
        serviceMock.Setup(svc => svc.GetStatus()).Returns("Healthy");
        var controller = new HealthController(serviceMock.Object);

        var result = controller.GetStatus();

        var ok = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(ok.Value);
        using var document = JsonDocument.Parse(json);

        Assert.Equal("Healthy", document.RootElement.GetProperty("status").GetString());
    }
}
