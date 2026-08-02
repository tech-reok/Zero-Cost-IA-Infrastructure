using CsharpAppBuildDocker.Api.Models;

namespace CsharpAppBuildDocker.Api.Repositories;

public interface IUserRepository
{
    IReadOnlyList<User> GetAll();
    User? GetById(int id);
}
