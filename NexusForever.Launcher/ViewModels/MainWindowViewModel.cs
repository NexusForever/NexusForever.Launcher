using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using NexusForever.Launcher.Services;

namespace NexusForever.Launcher.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    public string Title => $"NexusForever Launcher v{_updateService.GetVersion()}";

    public ServerListControlViewModel ServerListControlViewModel { get; }
    public ServerRepositoriesControlViewModel ServerRepositoriesControlViewModel { get; }
    public GameControlViewModel GameControlViewModel { get; }

    public ICommand OnLoadCommand => _onLoadCommand ??= new AsyncRelayCommand(OnLoad);
    private ICommand _onLoadCommand;

    #region Dependency Injection

    private readonly IUpdateService _updateService;
    private readonly IDialogCoordinator _dialogCoordinator;

    public MainWindowViewModel(
        IUpdateService updateService,
        IDialogCoordinator dialogCoordinator,
        ServerListControlViewModel serverListControlViewModel,
        ServerRepositoriesControlViewModel serverRepositoriesControlViewModel,
        GameControlViewModel gameControlViewModel)
    {
        _updateService     = updateService;
        _dialogCoordinator = dialogCoordinator;

        ServerListControlViewModel         = serverListControlViewModel;
        ServerRepositoriesControlViewModel = serverRepositoriesControlViewModel;
        GameControlViewModel               = gameControlViewModel;
    }

    #endregion

    public MainWindowViewModel()
    {
    }

    private async Task Update()
    {
        try
        {
            var version = await _updateService.GetUpdate();
            if (version == null)
                return;

            if (version.Version == _updateService.GetVersion())
                return;

            MessageDialogResult result = await _dialogCoordinator.ShowMessageAsync(this, "Update Available",
                    $"""
                    An update (v{version.Version}) is available for the NexusForever Launcher.
                    Would you like to download it?
                    """, MessageDialogStyle.AffirmativeAndNegative);

            if (result != MessageDialogResult.Affirmative)
                return;

            Process.Start(new ProcessStartInfo
            {
                FileName = version.Url,
                UseShellExecute = true
            });
        }
        catch (Exception e)
        {
            await _dialogCoordinator.ShowMessageAsync(this, "Update Error", e.Message);
        }
    }

    private async Task OnLoad()
    {
        await Update();

        await ServerRepositoriesControlViewModel.OnLoad();
        await ServerListControlViewModel.OnLoad();
        await GameControlViewModel.OnLoad();
    }
}
