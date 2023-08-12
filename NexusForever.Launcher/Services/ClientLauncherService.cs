using System.IO;
using System.Runtime.InteropServices;
using NexusForever.Launcher.Models;
using NexusForever.Launcher.Models.Native;

namespace NexusForever.Launcher.Services;

public class ClientLauncherService : IClientLauncherService
{
    [DllImport("kernel32.dll")]
    static extern bool CreateProcess(
        string lpApplicationName,
        string lpCommandLine,
        nint lpProcessAttributes,
        nint lpThreadAttributes,
        bool bInheritHandles,
        uint dwCreationFlags,
        nint lpEnvironment,
        string lpCurrentDirectory,
        ref STARTUPINFO lpStartupInfo,
        out PROCESS_INFORMATION lpProcessInformation
    );

    /// <summary>
    /// Launches the WildStar client with the supplied parameters.
    /// </summary>
    public void Launch(string clientLocation, LanguageModel language, ArchitectureModel architecture, ServerModel server)
    {
        string path = Path.Combine(clientLocation, architecture.Folder, architecture.Executable);
        string parameters = $"/auth {server.Address} /authNc {server.Address} /lang {language.LanguageCode} /patcher {server.Address} /SettingsKey WildStar /realmDataCenterId 9";

        var si = new STARTUPINFO();
        var pi = new PROCESS_INFORMATION();
        CreateProcess(path, parameters, nint.Zero, nint.Zero, false, 0, nint.Zero, null, ref si, out pi);
    }
}
