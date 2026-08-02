namespace CsharpAppBuildDocker.Api.Services;

public interface IHealthService
{
    string GetStatus();
    string GetCommitHash();
    string GetVersion() => "1.0.0";
}
