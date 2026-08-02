using CsharpAppBuildDocker.Api.Models;

namespace CsharpAppBuildDocker.Api.Repositories;

public sealed class InMemoryUserRepository : IUserRepository
{
    private static readonly IReadOnlyList<User> Users = new List<User>
    {
        new() { Id = 1, Name = "Alice Johnson", Address = "123 Main St, New York, NY", Associates = new List<string> { "Bob Smith", "Carol White" } },
        new() { Id = 2, Name = "Bob Smith", Address = "456 Elm St, Los Angeles, CA", Associates = new List<string> { "Alice Johnson", "David Brown" } },
        new() { Id = 3, Name = "Carol White", Address = "789 Oak St, Chicago, IL", Associates = new List<string> { "Alice Johnson", "Eva Green" } },
        new() { Id = 4, Name = "David Brown", Address = "321 Pine St, Houston, TX", Associates = new List<string> { "Bob Smith", "Frank Wilson" } },
        new() { Id = 5, Name = "Eva Green", Address = "654 Cedar St, Phoenix, AZ", Associates = new List<string> { "Carol White", "Grace Lee" } },
        new() { Id = 6, Name = "Frank Wilson", Address = "987 Maple St, Philadelphia, PA", Associates = new List<string> { "David Brown", "Henry Clark" } },
        new() { Id = 7, Name = "Grace Lee", Address = "147 Birch St, San Antonio, TX", Associates = new List<string> { "Eva Green", "Ivy Young" } },
        new() { Id = 8, Name = "Henry Clark", Address = "258 Walnut St, San Diego, CA", Associates = new List<string> { "Frank Wilson", "Jack Hall" } },
        new() { Id = 9, Name = "Ivy Young", Address = "369 Spruce St, Dallas, TX", Associates = new List<string> { "Grace Lee", "Jack Hall" } },
        new() { Id = 10, Name = "Jack Hall", Address = "741 Cherry St, San Jose, CA", Associates = new List<string> { "Henry Clark", "Ivy Young" } }
    };

    public IReadOnlyList<User> GetAll() => Users;

    public User? GetById(int id) => Users.FirstOrDefault(user => user.Id == id);
}
