namespace NexusForever.Launcher.Services;

public interface IClientValidationService
{
    /// <summary>
    /// Attempt to validate the WildStar client at the supplied location.
    /// </summary>
    bool IsValid(string clientLocation);
}
