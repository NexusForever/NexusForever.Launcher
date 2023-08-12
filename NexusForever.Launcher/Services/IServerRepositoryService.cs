using System.Collections.Generic;
using System.Threading.Tasks;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Services;

public interface IServerRepositoryService
{
    /// <summary>
    /// Load and add existing repositories to the collection.
    /// </summary>
    Task Load();

    /// <summary>
    /// Add a new repository to the collection.
    /// This will populate the <see cref="ServerRepositoryModel"/> from the supplied url.
    /// </summary>
    Task<ServerRepositoryModel> AddRepository(string url);

    /// <summary>
    /// Remove an existing <see cref="ServerRepositoryModel"/> from the collection.
    /// </summary>
    Task RemoveRepository(ServerRepositoryModel serverRepository);

    /// <summary>
    /// Get <see cref="ServerRepositoryModel"/> collection.
    /// </summary>
    IEnumerable<ServerRepositoryModel> GetRepositories();
}