using System;

namespace NexusForever.Launcher.Models;

public class LanguageModel
{
    public string LanguageCode
    {
        get => _languageCode;
        set
        {
            _languageCode = value;

            Language = value switch
            {
                "EN" => "English",
                "DE" => "German",
                "FR" => "French",
                _    => throw new NotImplementedException("Invalid Language")
            };
        }
    }

    private string _languageCode;

    public string Language { get; set; }
}
