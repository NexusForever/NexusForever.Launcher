using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NexusForever.Launcher.Configuration;

namespace NexusForever.Launcher.Repositories;

public class ServerRepositoryRepository : IServerRepositoryRepository
{
    private List<string> _respositories;

    #region Dependency Injection

    private readonly IOptions<LauncherConfiguration> _launcherOptions;

    public ServerRepositoryRepository(
        IOptions<LauncherConfiguration> launcherOptions)
    {
        _launcherOptions = launcherOptions;
    }

    #endregion

    /// <summary>
    /// Add repository url to the repository.
    /// </summary>
    public async Task AddRepository(string url)
    {
        await Load();
        _respositories.Add(url);
        await Save();
    }

    /// <summary>
    /// Remove repository url from the repository.
    /// </summary>
    public async Task RemoveRepository(string url)
    {
        await Load();
        _respositories.Remove(url);
        await Save();
    }

    /// <summary>
    /// Get repository url collection from the repository.
    /// </summary>
    public async Task<IEnumerable<string>> GetRepositories()
    {
        await Load();
        return _respositories;
    }

    private async Task Load()
    {
        if (_respositories != null)
            return;

        if (!File.Exists(_launcherOptions.Value.RepositoryFile))
        {
            _respositories = new();
            return;
        }

        using FileStream stream = File.Open(_launcherOptions.Value.RepositoryFile, FileMode.Open, FileAccess.Read);
        _respositories = await JsonSerializer.DeserializeAsync<List<string>>(stream);
    }

    private async Task Save()
    {
        using FileStream stream = File.Open(_launcherOptions.Value.RepositoryFile, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(stream, _respositories, options: new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
