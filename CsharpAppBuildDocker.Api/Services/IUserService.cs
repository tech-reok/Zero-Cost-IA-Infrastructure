namespace CsharpAppBuildDocker.Api.Services;

public interface IUserService
{
    IReadOnlyList<string> GetUserNames();
    string? GetUserAddress(int id);
    IReadOnlyList<string>? GetAssociates(int id);
}
