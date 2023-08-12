using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Services;

public interface IClientInformationService
{
    /// <summary>
    /// Gather client information about the WildStar client at the specified location.
    /// </summary>
    ClientInformationModel GetInformation(string clientLocation);
}
