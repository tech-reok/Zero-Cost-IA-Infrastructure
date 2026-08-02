using CsharpAppBuildDocker.Api.Models;
using CsharpAppBuildDocker.Api.Repositories;
using CsharpAppBuildDocker.Api.Services;
using Moq;

namespace CsharpAppBuildDocker.Api.Tests.Services;

public sealed class UserServiceTests
{
    [Fact]
    public void GetUserNames_WhenUsersExist_ReturnsAllNames()
    {
        var users = new List<User>
        {
            new() { Id = 1, Name = "Alice", Address = "Address 1", Associates = new List<string> { "Bob" } },
            new() { Id = 2, Name = "Bob", Address = "Address 2", Associates = new List<string> { "Alice" } }
        };

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock.Setup(repo => repo.GetAll()).Returns(users);

        var service = new UserService(repositoryMock.Object);

        var result = service.GetUserNames();

        Assert.Equal(2, result.Count);
        Assert.Equal("Alice", result[0]);
        Assert.Equal("Bob", result[1]);
    }

    [Fact]
    public void GetUserNames_WhenNoUsers_ReturnsEmptyList()
    {
        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock.Setup(repo => repo.GetAll()).Returns(Array.Empty<User>());

        var service = new UserService(repositoryMock.Object);

        var result = service.GetUserNames();

        Assert.Empty(result);
    }

    [Fact]
    public void GetUserAddress_WhenUserExists_ReturnsAddress()
    {
        var user = new User { Id = 10, Name = "User", Address = "10 Main St", Associates = new List<string> { "A" } };
        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock.Setup(repo => repo.GetById(10)).Returns(user);

        var service = new UserService(repositoryMock.Object);

        var result = service.GetUserAddress(10);

        Assert.Equal("10 Main St", result);
    }

    [Fact]
    public void GetUserAddress_WhenUserDoesNotExist_ReturnsNull()
    {
        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock.Setup(repo => repo.GetById(999)).Returns((User?)null);

        var service = new UserService(repositoryMock.Object);

        var result = service.GetUserAddress(999);

        Assert.Null(result);
    }

    [Fact]
    public void GetAssociates_WhenUserExists_ReturnsAssociates()
    {
        var associates = new List<string> { "A", "B" };
        var user = new User { Id = 3, Name = "User", Address = "Address", Associates = associates };
        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock.Setup(repo => repo.GetById(3)).Returns(user);

        var service = new UserService(repositoryMock.Object);

        var result = service.GetAssociates(3);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains("A", result);
        Assert.Contains("B", result);
    }

    [Fact]
    public void GetAssociates_WhenUserDoesNotExist_ReturnsNull()
    {
        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock.Setup(repo => repo.GetById(404)).Returns((User?)null);

        var service = new UserService(repositoryMock.Object);

        var result = service.GetAssociates(404);

        Assert.Null(result);
    }
}
