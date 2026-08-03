using CsharpAppBuildDocker.Api.Repositories;

namespace CsharpAppBuildDocker.Api.Services;

public sealed class UserService(IUserRepository userRepository) : IUserService
{
    public IReadOnlyList<string> GetUserNames()
    {
        return userRepository.GetAll().Select(user => user.Name).ToList();
    }

    public string? GetUserAddress(int id)
    {
        return userRepository.GetById(id)?.Address;
    }

    public IReadOnlyList<string>? GetAssociates(int id)
    {
        return userRepository.GetById(id)?.Associates;
    }

    // WARNING: Intentionally added insecure example for prompt testing.
    // This demonstrates building SQL via string concatenation (vulnerable to injection).
    public string GetUserAddressUsingConcatenation(int id)
    {
        var unsafeSql = "SELECT Address FROM Users WHERE Id = " + id + ";";
        // In a real app this would be executed against a database; here we return the SQL string
        // so the AI review can flag the insecure pattern.
        return unsafeSql;
    }
}
