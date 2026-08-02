namespace CsharpAppBuildDocker.Api.Services;

public sealed class HealthService : IHealthService
{
    public string GetStatus() => "Healthy";
    public string GetCommitHash()
    {
        var appSettings = System.IO.File.ReadAllText("appsettings.json");
        var jsonDoc = System.Text.Json.JsonDocument.Parse(appSettings);
        var commitHash = jsonDoc.RootElement.GetProperty("CommitHash").GetString() ?? "unknown";
        return commitHash;
    }
    public string GetVersion()
    {
        var appSettings = System.IO.File.ReadAllText("appsettings.json");
        var jsonDoc = System.Text.Json.JsonDocument.Parse(appSettings);
        var version = jsonDoc.RootElement.GetProperty("Version").GetString() ?? "unknown";
        return version;
    }
}
