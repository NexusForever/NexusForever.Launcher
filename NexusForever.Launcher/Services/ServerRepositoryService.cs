using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexusForever.Launcher.Models;
using NexusForever.Launcher.Repositories;

namespace NexusForever.Launcher.Services;

public class ServerRepositoryService : IServerRepositoryService
{
    private readonly List<ServerRepositoryModel> _repositories = new();

    #region Dependency Injection

    private readonly IServerRepositorySourceFactory _serverRepositorySourceFactory;
    private readonly IServerRepositoryRepository _serverRepositoryRepository;

    private readonly ICustomServerMessageFormatter _customServerMessageFormatter;

    public ServerRepositoryService(
        IServerRepositorySourceFactory serverRepositorySourceFactory,
        IServerRepositoryRepository serverRepositoryRepository,
        ICustomServerMessageFormatter customServerMessageFormatter)
    {
        _serverRepositorySourceFactory = serverRepositorySourceFactory;
        _serverRepositoryRepository    = serverRepositoryRepository;

        _customServerMessageFormatter  = customServerMessageFormatter;
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
        IServerRepositorySource source = _serverRepositorySourceFactory.Create(url);

        ServerRepositoryModel serverRepository = await source.GetRepository(url);
        EnrichRepository(url, serverRepository);

        return serverRepository;
    }

    private void EnrichRepository(string url, ServerRepositoryModel serverRepository)
    {
        serverRepository.Url = url;

        foreach (ServerModel server in serverRepository.Servers.Where(s => s.Custom))
            _customServerMessageFormatter.Format(serverRepository, server);
    }
}
