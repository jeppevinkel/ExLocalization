# ExLocalization
Centralized localization for EXILED plugins.

## Usage
Example usage of the library
```cs
public class Plugin : Plugin<Config>
{
  internal static Translation MyTranslation;
  
  public override void OnEnabled()
  {
    this.RegisterTranslation(new Translation(), "en");
    
    MyTranslation = this.LoadTranslation<Translation>();
    
    Log.Debug(MyTranslation.SampleMessage);
  }
}

public class Translation
{
  public string SampleMessage { get; set; } = "This is in english!";
}
```

## API
```cs
// Registers a translation with the given language for the plugin. (Adds missing strings to old files, but doesn't override existing strings)
RegisterTranslation<T>(this IPlugin<IConfig> plugin, T translation, string language)

// Returns the translation file for usage. (Please cache the result as the function is a bit taxing to run often)
LoadTranslation<T>(this IPlugin<IConfig> plugin)

// Returns all info on the currently selected language.
GetLanguage()
```
