using System.Threading.Tasks;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Repositories
{
    public interface IServerRepositorySource
    {
        Task<ServerRepositoryModel> GetRepository(string url);
    }
}
