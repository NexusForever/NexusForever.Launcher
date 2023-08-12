using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAPICodePack.Dialogs;
using NexusForever.Launcher.Configuration;
using NexusForever.Launcher.Models;
using NexusForever.Launcher.Repositories;
using NexusForever.Launcher.Services;

namespace NexusForever.Launcher.ViewModels;

public partial class GameControlViewModel : ObservableObject
{
    [ObservableProperty]
    private string _clientLocation;

    public ObservableCollection<ArchitectureModel> AvailableArchitectures { get; set; } = new();

    [ObservableProperty]
    private ArchitectureModel _selectedArchitecture;

    public ObservableCollection<LanguageModel> AvailableLanguages { get; set; } = new();

    [ObservableProperty]
    private LanguageModel _selectedLanguage;

    public ICommand OnBrowseClient => new AsyncRelayCommand(BrowseClient);

    #region Dependency Injection

    private readonly IOptions<LauncherConfiguration> _launcherOptions;
    private readonly IClientRepository _clientRepository;

    private readonly IClientFinderService _clientFinderService;
    private readonly IClientValidationService _clientValidationService;
    private readonly IClientInformationService _clientInformationService;

    private readonly IDialogCoordinator _dialogCoordinator;

    public GameControlViewModel(
        IOptions<LauncherConfiguration> launcherOptions,
        IClientRepository clientRepository,
        IClientFinderService clientFinderService,
        IClientValidationService clientValidationService,
        IClientInformationService clientInformationService,
        IDialogCoordinator dialogCoordinator)
    {
        _launcherOptions          = launcherOptions;
        _clientRepository         = clientRepository;

        _clientFinderService      = clientFinderService;
        _clientValidationService  = clientValidationService;
        _clientInformationService = clientInformationService;

        _dialogCoordinator        = dialogCoordinator;
    }

    #endregion

    public GameControlViewModel()
    {
    }

    private void PopulateClientInformation()
    {
        AvailableLanguages.Clear();
        AvailableArchitectures.Clear();

        ClientInformationModel information = _clientInformationService.GetInformation(ClientLocation);
        foreach (ArchitectureModel architecture in information.AvailableArchitectures)
            AvailableArchitectures.Add(architecture);

        if (AvailableArchitectures.Count > 0)
        {
            SelectedArchitecture = information.AvailableArchitectures
                // check if we have a prefered architecture set in the config
                .SingleOrDefault(a => a.Architecture.Equals(_launcherOptions.Value.DefaultArchitecture, StringComparison.CurrentCultureIgnoreCase))
                // if not, check if we have 64bit architecture available
                ?? information.AvailableArchitectures.SingleOrDefault(a => a.Architecture == "64bit")
                // if not, just take the first one
                ?? information.AvailableArchitectures[0];
        }

        foreach (LanguageModel language in information.AvailableLanguages)
            AvailableLanguages.Add(language);

        if (AvailableLanguages.Count > 0)
        {
            SelectedLanguage = information.AvailableLanguages
                // check if we have a prefered language set in the config
                .SingleOrDefault(l => l.Language.Equals(_launcherOptions.Value.DefaultLanguage, StringComparison.CurrentCultureIgnoreCase))
                // if not, check if we have English language available
                ?? information.AvailableLanguages.SingleOrDefault(a => a.Language == "English")
                // if not, just take the first one
                ?? information.AvailableLanguages[0];
        }
    }

    /// <summary>
    /// Invoked on main window load.
    /// </summary>
    public async Task OnLoad()
    {
        ClientLocation = await _clientRepository.GetClient() ?? _clientFinderService.GetClient();
        if (ClientLocation != null && _clientValidationService.IsValid(ClientLocation))
            PopulateClientInformation();
    }

    private async Task BrowseClient()
    {
        var dialog = new CommonOpenFileDialog
        {
            Title            = "Select WildStar Client Folder",
            InitialDirectory = ClientLocation,
            IsFolderPicker   = true
        };

        CommonFileDialogResult result = dialog.ShowDialog();
        if (result != CommonFileDialogResult.Ok)
            return;

        if (!_clientValidationService.IsValid(dialog.FileName))
        {
            await _dialogCoordinator.ShowMessageAsync(this, "Invalid Client", "The selected folder does not contain a valid WildStar client.");
            return;
        }

        ClientLocation = dialog.FileName;
        await _clientRepository.UpdateClient(ClientLocation);

        PopulateClientInformation();
    }
}
