using System.Collections.Generic;

namespace NexusForever.Launcher.Models;

public class ServerRepositoryModel
{
    public string Url { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CustomMessage { get; set; }
    public List<ServerModel> Servers { get; set; }
}
