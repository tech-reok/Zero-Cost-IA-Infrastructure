using CsharpAppBuildDocker.Api.Repositories;

namespace CsharpAppBuildDocker.Api.Tests.Repositories;

public sealed class InMemoryUserRepositoryTests
{
    [Fact]
    public void GetAll_ReturnsAllSeededUsers()
    {
        var repository = new InMemoryUserRepository();

        var users = repository.GetAll();

        Assert.Equal(10, users.Count);
    }

    [Fact]
    public void GetById_WhenUserExists_ReturnsUser()
    {
        var repository = new InMemoryUserRepository();

        var user = repository.GetById(1);

        Assert.NotNull(user);
        Assert.Equal(1, user.Id);
        Assert.Equal("Alice Johnson", user.Name);
    }

    [Fact]
    public void GetById_WhenUserDoesNotExist_ReturnsNull()
    {
        var repository = new InMemoryUserRepository();

        var user = repository.GetById(999);

        Assert.Null(user);
    }

    [Fact]
    public void GetAll_ContainsExpectedEssentialFields()
    {
        var repository = new InMemoryUserRepository();

        var users = repository.GetAll();

        Assert.All(users, user =>
        {
            Assert.True(user.Id > 0);
            Assert.False(string.IsNullOrWhiteSpace(user.Name));
            Assert.False(string.IsNullOrWhiteSpace(user.Address));
            Assert.NotNull(user.Associates);
        });
    }
}
