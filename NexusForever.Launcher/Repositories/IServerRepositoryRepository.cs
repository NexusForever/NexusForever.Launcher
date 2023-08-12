using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusForever.Launcher.Repositories;

public interface IServerRepositoryRepository
{
    /// <summary>
    /// Add repository url to the repository.
    /// </summary>
    Task AddRepository(string url);

    /// <summary>
    /// Remove repository url from the repository.
    /// </summary>
    Task RemoveRepository(string url);

    /// <summary>
    /// Get repository url collection from the repository.
    /// </summary>
    Task<IEnumerable<string>> GetRepositories();
}
