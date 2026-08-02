namespace CsharpAppBuildDocker.Api.Models;

public sealed class User
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required IReadOnlyList<string> Associates { get; init; }
}
