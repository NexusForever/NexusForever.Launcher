using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexusForever.Launcher.Models;

namespace NexusForever.Launcher.ViewModels;

public class AddServerControlViewModel : ObservableObject
{
    public readonly TaskCompletionSource<ServerModel> TaskCompletionSource = new();

    public string ServerName
    {
        get => _serverName;
        set
        {
            _serverName = value;
            OnPropertyChanged(nameof(ServerName));

            OnAdd.NotifyCanExecuteChanged();
        }
    }

    private string _serverName;

    public string ServerAddress
    {
        get => _serverAddress;
        set
        {
            _serverAddress = value;
            OnPropertyChanged(nameof(ServerAddress));

            OnAdd.NotifyCanExecuteChanged();
        }
    }

    private string _serverAddress;

    public RelayCommand OnAdd => _onAdd ??= new RelayCommand(Add, CanAdd);
    private RelayCommand _onAdd;

    public ICommand OnCancel => _onCancel ??= new RelayCommand(Cancel);
    private ICommand _onCancel;

    private bool CanAdd()
    {
        return !string.IsNullOrEmpty(ServerName) && !string.IsNullOrEmpty(ServerAddress);
    }

    private void Add()
    {
        TaskCompletionSource.SetResult(new ServerModel
        {
            Name    = ServerName,
            Address = ServerAddress,
            Local   = true
        });
    }

    private void Cancel()
    {
        TaskCompletionSource.SetResult(null);
    }
}
