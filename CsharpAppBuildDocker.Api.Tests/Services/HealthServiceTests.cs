using CsharpAppBuildDocker.Api.Services;

namespace CsharpAppBuildDocker.Api.Tests.Services;

public sealed class HealthServiceTests
{
    [Fact]
    public void GetStatus_ReturnsHealthy()
    {
        var service = new HealthService();

        var result = service.GetStatus();

        Assert.Equal("Healthy", result);
    }
}
