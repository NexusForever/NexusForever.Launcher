using System.Collections.Generic;
using System.Threading.Tasks;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Repositories;

public interface IServerRepository
{
    /// <summary>
    /// Add <see cref="ServerModel"/> to the repository.
    /// </summary>
    Task AddServer(ServerModel server);

    /// <summary>
    /// Remove <see cref="ServerModel"/> from the repository.
    /// </summary>
    Task RemoveServer(ServerModel server);

    /// <summary>
    /// Get <see cref="ServerModel"/> collection from the repository.
    /// </summary>
    Task<IEnumerable<ServerModel>> GetServers();
}
