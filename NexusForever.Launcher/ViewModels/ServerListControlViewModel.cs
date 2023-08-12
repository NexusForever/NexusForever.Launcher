using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using NexusForever.Launcher.Models;
using NexusForever.Launcher.Repositories;
using NexusForever.Launcher.Services;

namespace NexusForever.Launcher.ViewModels;

public class ServerListControlViewModel : ObservableObject
{
    public GameControlViewModel GameControlViewModel { get; set; }

    public ObservableCollection<ServerModel> Servers { get; set; } = new();

    public ServerModel SelectedServer
    {
        get => _selectedServer;
        set
        {
            _selectedServer = value;
            OnPropertyChanged(nameof(SelectedServer));

            OnRemoveServer.NotifyCanExecuteChanged();
            OnPlayServer.NotifyCanExecuteChanged();
        }
    }

    private ServerModel _selectedServer;

    public ICommand OnWebsiteCommand => _onWebsiteCommand ??= new AsyncRelayCommand<ServerModel>(Website);
    private ICommand _onWebsiteCommand;

    public ICommand OnDiscordCommand => _onDiscordCommand ??= new AsyncRelayCommand<ServerModel>(Discord);
    private ICommand _onDiscordCommand;

    public ICommand OnAddServer => _onAddServer ??= new AsyncRelayCommand(AddServer);
    private ICommand _onAddServer;

    public AsyncRelayCommand OnRemoveServer => _onRemoveServer ??= new AsyncRelayCommand(RemoveServer, CanRemoveServer);
    private AsyncRelayCommand _onRemoveServer;

    public AsyncRelayCommand OnPlayServer => _onPlayServer ??= new AsyncRelayCommand(PlayServer, CanPlayServer);
    private AsyncRelayCommand _onPlayServer;

    #region Dependency Injection

    private readonly IServerRepository _serverRepository;

    private readonly IDialogCoordinator _dialogCoordinator;
    private readonly ICustomDialogService _customDialogService;
    private readonly IClientLauncherService _clientLauncherService;

    public ServerListControlViewModel(
        GameControlViewModel gameControlViewModel,
        IServerRepository serverRepository,
        IDialogCoordinator dialogCoordinator,
        ICustomDialogService customDialogService,
        IClientLauncherService clientLauncherService)
    {
        GameControlViewModel   = gameControlViewModel;

        _serverRepository      = serverRepository;

        _dialogCoordinator     = dialogCoordinator;
        _customDialogService   = customDialogService;
        _clientLauncherService = clientLauncherService;
    }

    #endregion

    public ServerListControlViewModel()
    {
    }

    /// <summary>
    /// Invoked on main window load.
    /// </summary>
    public async Task OnLoad()
    {
        foreach (ServerModel server in await _serverRepository.GetServers())
            Servers.Add(server);
    }

    private async Task Website(ServerModel server)
    {
        await LaunchCommunity(server.Website, $"""
            You are about to visit a community hosted website.
            Do you wish to continue?

            Discord: {server.Website}
            """);
    }

    private async Task Discord(ServerModel server)
    {
        await LaunchCommunity(server.Discord, $"""
            You are about to visit a community hosted Discord server.
            Do you wish to continue?

            Discord: {server.Discord}
            """);
    }

    private async Task LaunchCommunity(string url, string message)
    {
        MessageDialogResult result = await _dialogCoordinator.ShowMessageAsync(this, "Launch Server", message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "Yes", NegativeButtonText = "No" });
        if (result != MessageDialogResult.Affirmative)
            return;

        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }

    private async Task AddServer()
    {
        ServerModel server = await _customDialogService.ShowAddServerDialog(this);
        if (server == null)
            return;

        await _serverRepository.AddServer(server);
        Servers.Add(server);
    }

    private bool CanRemoveServer()
    {
        return SelectedServer?.Local ?? false;
    }

    private async Task RemoveServer()
    {
        await _serverRepository.RemoveServer(SelectedServer);
        Servers.Remove(SelectedServer);
    }

    private bool CanPlayServer()
    {
        return SelectedServer != null;
    }

    private async Task PlayServer()
    {
        if (GameControlViewModel.ClientLocation == null)
        {
            await _dialogCoordinator.ShowMessageAsync(this, "Launch Server", """
                No WildStar client was found.
                Manually select the client location in the Game tab.
                """);
            return;
        }

        if (SelectedServer.Custom)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"""
                {SelectedServer.Name} has been marked as a custom server, it might require additional client setup before playing.
                If you encounter any issues contact the community administration for further information.
                """);

            sb.AppendLine();
            if (SelectedServer.Discord != null)
                sb.AppendLine($"Discord: {SelectedServer.Discord}");
            if (SelectedServer.Website != null)
                sb.AppendLine($"Website: {SelectedServer.Website}");

            await _dialogCoordinator.ShowMessageAsync(this, "Launch Server", sb.ToString());
        }

        _clientLauncherService.Launch(GameControlViewModel.ClientLocation, GameControlViewModel.SelectedLanguage, GameControlViewModel.SelectedArchitecture, SelectedServer);
    }
}
