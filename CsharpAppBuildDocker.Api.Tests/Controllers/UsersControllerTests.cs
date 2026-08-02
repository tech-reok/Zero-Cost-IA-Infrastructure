using System.Text.Json;
using CsharpAppBuildDocker.Api.Controllers;
using CsharpAppBuildDocker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CsharpAppBuildDocker.Api.Tests.Controllers;

public sealed class UsersControllerTests
{
    [Fact]
    public void GetUserNames_ReturnsOkWithNames()
    {
        var serviceMock = new Mock<IUserService>();
        serviceMock.Setup(svc => svc.GetUserNames()).Returns(new List<string> { "Alice", "Bob" });
        var controller = new UsersController(serviceMock.Object);

        var result = controller.GetUserNames();

        var ok = Assert.IsType<OkObjectResult>(result);
        var names = Assert.IsAssignableFrom<IReadOnlyList<string>>(ok.Value);
        Assert.Equal(2, names.Count);
        Assert.Contains("Alice", names);
        Assert.Contains("Bob", names);
    }

    [Fact]
    public void GetUserAddress_WhenUserExists_ReturnsOk()
    {
        var serviceMock = new Mock<IUserService>();
        serviceMock.Setup(svc => svc.GetUserAddress(7)).Returns("147 Birch St, San Antonio, TX");
        var controller = new UsersController(serviceMock.Object);

        var result = controller.GetUserAddress(7);

        var ok = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(ok.Value);
        using var document = JsonDocument.Parse(json);

        Assert.Equal(7, document.RootElement.GetProperty("id").GetInt32());
        Assert.Equal("147 Birch St, San Antonio, TX", document.RootElement.GetProperty("address").GetString());
    }

    [Fact]
    public void GetUserAddress_WhenUserMissing_ReturnsNotFound()
    {
        var serviceMock = new Mock<IUserService>();
        serviceMock.Setup(svc => svc.GetUserAddress(777)).Returns((string?)null);
        var controller = new UsersController(serviceMock.Object);

        var result = controller.GetUserAddress(777);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        var json = JsonSerializer.Serialize(notFound.Value);
        using var document = JsonDocument.Parse(json);

        Assert.Equal("User not found.", document.RootElement.GetProperty("message").GetString());
    }

    [Fact]
    public void GetAssociates_WhenUserExists_ReturnsOk()
    {
        var serviceMock = new Mock<IUserService>();
        serviceMock.Setup(svc => svc.GetAssociates(3)).Returns(new List<string> { "Alice Johnson", "Eva Green" });
        var controller = new UsersController(serviceMock.Object);

        var result = controller.GetAssociates(3);

        var ok = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(ok.Value);
        using var document = JsonDocument.Parse(json);

        Assert.Equal(3, document.RootElement.GetProperty("id").GetInt32());
        Assert.Equal(2, document.RootElement.GetProperty("associates").GetArrayLength());
    }

    [Fact]
    public void GetAssociates_WhenUserMissing_ReturnsNotFound()
    {
        var serviceMock = new Mock<IUserService>();
        serviceMock.Setup(svc => svc.GetAssociates(404)).Returns((IReadOnlyList<string>?)null);
        var controller = new UsersController(serviceMock.Object);

        var result = controller.GetAssociates(404);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        var json = JsonSerializer.Serialize(notFound.Value);
        using var document = JsonDocument.Parse(json);

        Assert.Equal("User not found.", document.RootElement.GetProperty("message").GetString());
    }
}
