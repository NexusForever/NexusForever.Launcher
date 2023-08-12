using System.IO;

namespace NexusForever.Launcher.Services;

public class ClientValidationService : IClientValidationService
{
    /// <summary>
    /// Attempt to validate the WildStar client at the supplied location.
    /// </summary>
    public bool IsValid(string clientLocation)
    {
        // check if the patcher exists
        if (!File.Exists(Path.Combine(clientLocation, "WildStar.exe")))
            return false;

        // check if the index and archive directory exists
        if (!Directory.Exists(Path.Combine(clientLocation, "Patch")))
            return false;

        // check if either the 32 or 64 bit client exists
        if (!File.Exists(Path.Combine(clientLocation, "Client", "WildStar32.exe"))
            && !File.Exists(Path.Combine(clientLocation, "Client64", "WildStar64.exe")))
            return false;

        return true;
    }
}
