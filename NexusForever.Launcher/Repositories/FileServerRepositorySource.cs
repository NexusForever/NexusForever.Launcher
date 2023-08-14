using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Repositories
{
    internal class FileServerRepositorySource : IServerRepositorySource
    {
        public async Task<ServerRepositoryModel> GetRepository(string url)
        {
            await using FileStream fileStream = File.OpenRead($"{url.TrimEnd('/')}/Servers.json");
            return await JsonSerializer.DeserializeAsync<ServerRepositoryModel>(fileStream);
        }
    }
}
