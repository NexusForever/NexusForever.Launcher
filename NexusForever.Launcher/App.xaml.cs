using System;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusForever.Launcher.Configuration;
using NexusForever.Launcher.Repositories;
using NexusForever.Launcher.Services;
using NexusForever.Launcher.ViewModels;
using NexusForever.Launcher.Views.Windows;

namespace NexusForever.Launcher;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IConfiguration _configuration;
    private IServiceProvider _serviceProvider;

    public App()
    {
        var configuration = new ConfigurationBuilder();
        ConfigureConfiguration(configuration);
        _configuration = configuration.Build();

        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureConfiguration(IConfigurationBuilder configuration)
    {
        configuration.AddJsonFile("Configuration.json");
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();

        services.Configure<LauncherConfiguration>(_configuration);

        services.AddSingleton<MainWindow>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<ServerListControlViewModel>();
        services.AddSingleton<ServerRepositoriesControlViewModel>();
        services.AddSingleton<GameControlViewModel>();
        services.AddTransient<AddServerControlViewModel>();

        services.AddSingleton<IDialogCoordinator, DialogCoordinator>();
        services.AddTransient<ICustomDialogService, CustomDialogService>();

        services.AddTransient<IClientFinderService, ClientFinderService>();
        services.AddTransient<IClientInformationService, ClientInformationService>();
        services.AddTransient<IClientValidationService, ClientValidationService>();
        services.AddTransient<IClientLauncherService, ClientLauncherService>();

        services.AddSingleton<IServerRepositoryService, ServerRepositoryService>();

        services.AddTransient<IServerRepositoryRepository, ServerRepositoryRepository>();
        services.AddTransient<IClientRepository, ClientRepository>();
        services.AddTransient<IServerRepository, ServerRepository>();

        services.AddTransient<IServerRepositorySourceFactory, ServerRepositorySourceFactory>();
        services.AddTransient<HttpServerRepositorySource>();
        services.AddTransient<FileServerRepositorySource>();

        services.AddTransient<ICustomServerMessageFormatter, CustomServerMessageFormatter>();

        services.AddTransient<IUpdateService, UpdateService>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        MainWindow mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow.DataContext = _serviceProvider.GetService<MainWindowViewModel>();
        mainWindow.Show();

        base.OnStartup(e);
    }
}
