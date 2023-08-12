using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using NexusForever.Launcher.Models;
using NexusForever.Launcher.ViewModels;
using NexusForever.Launcher.Views;

namespace NexusForever.Launcher.Services;

public class CustomDialogService : ICustomDialogService
{
    #region Dependency Injection

    private readonly IServiceProvider _serviceProvider;
    private readonly IDialogCoordinator _dialogCoordinator;

    public CustomDialogService(
        IServiceProvider serviceProvider,
        IDialogCoordinator dialogCoordinator)
    {
        _serviceProvider   = serviceProvider;
        _dialogCoordinator = dialogCoordinator;
    }

    #endregion

    /// <summary>
    /// Show a custom dialog to add a new server.
    /// </summary>
    public async Task<ServerModel> ShowAddServerDialog(object context)
    {
        var vm = _serviceProvider.GetRequiredService<AddServerControlViewModel>();
        var dialog = new CustomDialog
        {
            Content = new AddServerControl(),
            DataContext = vm
        };

        await _dialogCoordinator.ShowMetroDialogAsync(context, dialog);
        ServerModel model = await vm.TaskCompletionSource.Task;
        await _dialogCoordinator.HideMetroDialogAsync(context, dialog);

        return model;
    }
}
