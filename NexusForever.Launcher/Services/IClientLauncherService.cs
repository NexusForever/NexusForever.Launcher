using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Services;

public interface IClientLauncherService
{
    /// <summary>
    /// Launches the WildStar client with the supplied parameters.
    /// </summary>
    void Launch(string clientLocation, LanguageModel language, ArchitectureModel architecture, ServerModel server);
}
