using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Services;

public class ClientInformationService : IClientInformationService
{
    /// <summary>
    /// Gather client information about the WildStar client at the specified location.
    /// </summary>
    public ClientInformationModel GetInformation(string clientLocation)
    {
        return new ClientInformationModel
        {
            Path                   = clientLocation,
            AvailableArchitectures = GetAvailableArchitectures(clientLocation).ToList(),
            AvailableLanguages     = GetAvailableLanguages(clientLocation).ToList()
        };
    }

    private IEnumerable<ArchitectureModel> GetAvailableArchitectures(string clientLocation)
    {
        // find all available client architectures
        foreach (string directory in Directory.EnumerateDirectories(clientLocation))
        {
            Match architectureMatch = Regex.Match(directory, @"Client[0-9]*");
            if (!architectureMatch.Success)
                continue;

            yield return new ArchitectureModel { Folder = architectureMatch.Value };
        }
    }

    private IEnumerable<LanguageModel> GetAvailableLanguages(string clientLocation)
    {
        // find all available client languages
        string patch = Path.Combine(clientLocation, "Patch");
        foreach (string file in Directory.EnumerateFiles(patch))
        {
            Match languageMatch = Regex.Match(file, @"ClientData([A-Z]{2}).*\.archive");
            if (!languageMatch.Success)
                continue;

            yield return new LanguageModel { LanguageCode = languageMatch.Groups[1].Value };
        }
    }
}
