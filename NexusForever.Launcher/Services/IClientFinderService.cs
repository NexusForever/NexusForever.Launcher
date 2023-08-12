namespace NexusForever.Launcher.Services;

public interface IClientFinderService
{
    /// <summary>
    /// Attempt to find the WildStar client reuturning the path.
    /// </summary>
    string GetClient();
}