using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NexusForever.Launcher.Models;
using NexusForever.Launcher.Repositories;

namespace NexusForever.Launcher.Services;

public class ServerRepositoryService : IServerRepositoryService
{
    private readonly List<ServerRepositoryModel> _repositories = new();

    #region Dependency Injection

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServerRepositoryRepository _serverRepositoryRepository;

    public ServerRepositoryService(
        IHttpClientFactory httpClientFactory,
        IServerRepositoryRepository serverRepositoryRepository)
    {
        _httpClientFactory          = httpClientFactory;
        _serverRepositoryRepository = serverRepositoryRepository;
    }

    #endregion

    /// <summary>
    /// Load and add existing repositories to the collection.
    /// </summary>
    public async Task Load()
    {
        _repositories.Clear();

        foreach (string url in await _serverRepositoryRepository.GetRepositories())
        {
            ServerRepositoryModel serverRepository = await CreateRepository(url);
            _repositories.Add(serverRepository);
        }
    }

    /// <summary>
    /// Add a new repository to the collection.
    /// This will populate the <see cref="ServerRepositoryModel"/> from the supplied url.
    /// </summary>
    public async Task<ServerRepositoryModel> AddRepository(string url)
    {
        ServerRepositoryModel serverRepository = await CreateRepository(url);
        _repositories.Add(serverRepository);

        await _serverRepositoryRepository.AddRepository(url);

        return serverRepository;
    }

    /// <summary>
    /// Remove an existing <see cref="ServerRepositoryModel"/> from the collection.
    /// </summary>
    public async Task RemoveRepository(ServerRepositoryModel serverRepository)
    {
        _repositories.Remove(serverRepository);
        await _serverRepositoryRepository.RemoveRepository(serverRepository.Url);
    }

    /// <summary>
    /// Get <see cref="ServerRepositoryModel"/> collection.
    /// </summary>
    public IEnumerable<ServerRepositoryModel> GetRepositories()
    {
        return _repositories;
    }

    private async Task<ServerRepositoryModel> CreateRepository(string url)
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        ServerRepositoryModel serverRepository = await client.GetFromJsonAsync<ServerRepositoryModel>($"{url.TrimEnd('/')}/Servers.json");
        serverRepository.Url = url;
        return serverRepository;
    }
}
