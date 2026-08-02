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
}
