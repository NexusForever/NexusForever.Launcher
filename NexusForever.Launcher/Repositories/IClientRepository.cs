using System.Threading.Tasks;

namespace NexusForever.Launcher.Repositories;

public interface IClientRepository
{
    /// <summary>
    /// Get client location from repository.
    /// </summary>
    Task<string> GetClient();

    /// <summary>
    /// Update client location in repository.
    /// </summary>
    Task UpdateClient(string client);
}
