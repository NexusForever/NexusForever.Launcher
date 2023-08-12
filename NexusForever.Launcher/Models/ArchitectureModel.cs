using System;

namespace NexusForever.Launcher.Models;

public class ArchitectureModel
{
    public string Folder
    {
        get => _folder;
        set
        {
            _folder = value;

            Architecture = value switch
            {
                "Client"   => "32bit",
                "Client64" => "64bit",
                _          => throw new NotImplementedException("Invalid architecture")
            };

            Executable = value switch
            {
                "Client"   => "Wildstar32.exe",
                "Client64" => "Wildstar64.exe",
                _          => throw new NotImplementedException("Invalid architecture")
            };
        }
    }

    private string _folder;

    public string Architecture { get; set; }

    public string Executable { get; set; } 
}
