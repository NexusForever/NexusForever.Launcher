using System.Collections.Generic;

namespace NexusForever.Launcher.Models;

public class ClientInformationModel
{
    public string Path { get; set; }
    public string Version { get; set; }
    public List<ArchitectureModel> AvailableArchitectures { get; set; } = new();
    public List<LanguageModel> AvailableLanguages { get; set; } = new();
}
