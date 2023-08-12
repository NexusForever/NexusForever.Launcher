using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NexusForever.Launcher.Configuration;

namespace NexusForever.Launcher.Repositories;

public class ClientRepository : IClientRepository
{
    private string _clientLocation;

    #region Dependency Injection

    private readonly IOptions<LauncherConfiguration> _launcherOptions;

    public ClientRepository(
        IOptions<LauncherConfiguration> launcherOptions)
    {
        _launcherOptions = launcherOptions;
    }

    #endregion

    /// <summary>
    /// Get client location from repository.
    /// </summary>
    public async Task<string> GetClient()
    {
        await Load();
        return _clientLocation;
    }

    /// <summary>
    /// Update client location in repository.
    /// </summary>
    public async Task UpdateClient(string clientLocation)
    {
        _clientLocation = clientLocation;
        await Save();
    }

    private async Task Load()
    {
        if (_clientLocation != null)
            return;

        if (!File.Exists(_launcherOptions.Value.ClientFile))
            return;

        using FileStream stream = File.Open(_launcherOptions.Value.ClientFile, FileMode.Open, FileAccess.Read);
        _clientLocation = await JsonSerializer.DeserializeAsync<string>(stream);
    }

    private async Task Save()
    {
        using FileStream stream = File.Open(_launcherOptions.Value.ClientFile, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(stream, _clientLocation, options: new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
