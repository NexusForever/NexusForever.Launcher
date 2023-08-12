using System.IO;
using Microsoft.Win32;

namespace NexusForever.Launcher.Services;

public class ClientFinderService : IClientFinderService
{
    /// <summary>
    /// Attempt to find the WildStar client reuturning the path.
    /// </summary>
    public string GetClient()
    {
        string path = FindNonSteam();
        path ??= FindSteam();
        return path;
    }

    private string FindNonSteam()
    {
        var registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\WildStar\");
        registryKey ??= Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\WildStar\");

        if (registryKey != null)
        {
            string value = (string)registryKey.GetValue("DisplayIcon");
            return Path.GetDirectoryName(value);
        }

        return null;
    }

    private string FindSteam()
    {
        // TODO: implement Steam client finder.
        return null;
    }
}
