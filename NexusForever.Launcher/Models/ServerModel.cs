using System.Text.Json.Serialization;

namespace NexusForever.Launcher.Models;

public class ServerModel
{
    [JsonIgnore]
    public bool HasWebsite => !string.IsNullOrEmpty(Website);

    [JsonIgnore]
    public bool HasDiscord => !string.IsNullOrEmpty(Discord);

    public bool Local { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public string Discord { get; set; }
    public bool Custom { get; set; }
}
