using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NexusForever.Launcher.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    public ServerListControlViewModel ServerListControlViewModel { get; }
    public ServerRepositoriesControlViewModel ServerRepositoriesControlViewModel { get; }
    public GameControlViewModel GameControlViewModel { get; }

    public ICommand OnLoadCommand => _onLoadCommand ??= new AsyncRelayCommand(OnLoad);
    private ICommand _onLoadCommand;

    public MainWindowViewModel(
        ServerListControlViewModel serverListControlViewModel,
        ServerRepositoriesControlViewModel serverRepositoriesControlViewModel,
        GameControlViewModel gameControlViewModel)
    {
        ServerListControlViewModel         = serverListControlViewModel;
        ServerRepositoriesControlViewModel = serverRepositoriesControlViewModel;
        GameControlViewModel               = gameControlViewModel;
    }

    public MainWindowViewModel()
    {
    }

    private async Task OnLoad()
    {
        await ServerRepositoriesControlViewModel.OnLoad();
        await ServerListControlViewModel.OnLoad();
        await GameControlViewModel.OnLoad();
    }
}
