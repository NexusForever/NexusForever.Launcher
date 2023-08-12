using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using NexusForever.Launcher.Models;
using NexusForever.Launcher.Services;

namespace NexusForever.Launcher.ViewModels;

public class ServerRepositoriesControlViewModel : ObservableObject
{
    public ServerListControlViewModel ServerListControlViewModel { get; set; }

    public ObservableCollection<ServerRepositoryModel> ServerRepositories { get; set; } = new();

    public ServerRepositoryModel SelectedServerRepository
    {
        get => _selectedServerRepository;
        set
        {
            _selectedServerRepository = value;
            OnPropertyChanged(nameof(SelectedServerRepository));

            OnRemoveRepository.NotifyCanExecuteChanged();
        }
    }
    
    private ServerRepositoryModel _selectedServerRepository;

    public ICommand OnAddRepository => _onAddRepository ??= new AsyncRelayCommand(AddRepository);
    private ICommand _onAddRepository;

    public AsyncRelayCommand OnRemoveRepository => _onRemoveRepository ??= new AsyncRelayCommand(RemoveRepository, CanRemoveRepository);
    private AsyncRelayCommand _onRemoveRepository;

    #region Dependency Injection

    private readonly IDialogCoordinator _dialogCoordinator;
    private readonly IServerRepositoryService _serverRepositoryService;

    public ServerRepositoriesControlViewModel(
        ServerListControlViewModel serverListControlViewModel,
        IDialogCoordinator dialogCoordinator,
        IServerRepositoryService serverRepositoryService)
    {
        ServerListControlViewModel = serverListControlViewModel;

        _dialogCoordinator         = dialogCoordinator;
        _serverRepositoryService   = serverRepositoryService;
    }

    #endregion

    public ServerRepositoriesControlViewModel()
    {
    }

    /// <summary>
    /// Invoked on main window load.
    /// </summary>
    public async Task OnLoad()
    {
        ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, "Server Repository", "Loading repository servers...");
        controller.SetIndeterminate();

        try
        {
            await _serverRepositoryService.Load();

            foreach (ServerRepositoryModel serverRepository in _serverRepositoryService.GetRepositories())
            {
                ServerRepositories.Add(serverRepository);
                foreach (ServerModel server in serverRepository.Servers)
                    ServerListControlViewModel.Servers.Add(server);
            }
        }
        catch (Exception exception)
        {
            await _dialogCoordinator.ShowMessageAsync(this, "Server Repository", $"A failure occured while retrieving repository servers: {exception.Message}");
        }
        finally
        {
            await controller.CloseAsync();
        }
    }

    private async Task AddRepository()
    {
        string url = await _dialogCoordinator.ShowInputAsync(this, "Server Repository", "Enter the URL of the server repository you wish to add:");
        if (string.IsNullOrEmpty(url))
            return;

        ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, "Server Repository", "Loading repository servers...");
        controller.SetIndeterminate();

        try
        {
            ServerRepositoryModel serverRepository = await _serverRepositoryService.AddRepository(url);
            ServerRepositories.Add(serverRepository);
            foreach (ServerModel server in serverRepository.Servers)
                ServerListControlViewModel.Servers.Add(server);
        }
        catch (Exception exception)
        {
            await _dialogCoordinator.ShowMessageAsync(this, "Server Repository", $"A failure occured while retrieving repository servers: {exception.Message}");
        }
        finally
        {
            await controller.CloseAsync();
        }
    }

    private bool CanRemoveRepository()
    {
        return SelectedServerRepository != null;
    }

    private async Task RemoveRepository()
    {
        await _serverRepositoryService.RemoveRepository(SelectedServerRepository);

        foreach (ServerModel server in SelectedServerRepository.Servers)
            ServerListControlViewModel.Servers.Remove(server);

        ServerRepositories.Remove(SelectedServerRepository);
    }
}
