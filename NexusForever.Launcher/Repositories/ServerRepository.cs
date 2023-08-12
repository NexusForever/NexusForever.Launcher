using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NexusForever.Launcher.Configuration;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.Repositories;

public class ServerRepository : IServerRepository
{
    private List<ServerModel> _servers;

    #region Dependency Injection

    private readonly IOptions<LauncherConfiguration> _launcherOptions;

    public ServerRepository(
        IOptions<LauncherConfiguration> launcherOptions)
    {
        _launcherOptions = launcherOptions;
    }

    #endregion

    /// <summary>
    /// Add <see cref="ServerModel"/> to the repository.
    /// </summary>
    public async Task AddServer(ServerModel server)
    {
        await Load();
        _servers.Add(server);
        await Save();
    }

    /// <summary>
    /// Remove <see cref="ServerModel"/> from the repository.
    /// </summary>
    public async Task RemoveServer(ServerModel server)
    {
        await Load();
        _servers.Remove(server);
        await Save();
    }

    /// <summary>
    /// Get <see cref="ServerModel"/> collection from the repository.
    /// </summary>
    public async Task<IEnumerable<ServerModel>> GetServers()
    {
        await Load();
        return _servers;
    }

    private async Task Load()
    {
        if (_servers != null)
            return;

        if (!File.Exists(_launcherOptions.Value.ServerFile))
        {
            _servers = new();
            return;
        }

        using FileStream stream = File.Open(_launcherOptions.Value.ServerFile, FileMode.Open, FileAccess.Read);
        _servers = await JsonSerializer.DeserializeAsync<List<ServerModel>>(stream);
    }

    private async Task Save()
    {
        using FileStream stream = File.Open(_launcherOptions.Value.ServerFile, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(stream, _servers, options: new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
