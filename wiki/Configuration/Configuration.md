# Configuration

**Structr.Configuration** package is intended to help organize settings in application.

## Installation

Configuration package is available on [NuGet](https://www.nuget.org/packages/Structr.Configuration/).

```
dotnet add package Structr.Configuration
```

## Setup

Create settings class.

```csharp
public class MySettings
{
    public string RootDirectory { get; set; }
    
    /* Other settings */
}
```

Configuration services uses different providers to get source settings. For example: JSON, XML file, Database, or something else.

You can create custom configuration provider:

```csharp
public class CustomConfigurationProvider<TSettings> : SettingsProvider<TSettings>
    where TSettings : class, new()
{
    public CustomConfigurationProvider(SettingsProviderOptions options, string path)
        : base(options, path)
    {
        /* Do some logic here */
    }
    
    protected override TSettings LoadSettings()
    {
        /* Do some logic here */
    }

    protected override void UpdateSettings(TSettings settings)
    {
        /* Do some logic here */
    }
}
```

And then setup configuration services:

```csharp
services.AddConfiguration()
    .AddProvider(new CustomConfigurationProvider<MySettings>());
```

Or you can use one of default implemented configuration provider from list:

* [JSON](#json-provider)
* [XML](#xml-provider)

### JSON provider

Create JSON file with settings:

```json
{
  "RootDirectory": "C:\\RootDirectory\\",
  /* Other settings */
}
```

Setup JSON configuration provider:

```csharp
services.AddConfiguration()
    .AddJson<MySettings>("path_to_json_file");
```

### XML provider

Create XML file with settings: 

```xml
<?xml version="1.0" encoding="utf-16"?>
<MySettings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <RootDirectory>C:\RootDirectory\</RootDirectory>
  /* Other settings */
</MySettings>
```

Setup XML configuration provider:

```csharp
services.AddConfiguration()
    .AddXml<MySettings>("path_to_xml_file");
```

### Options

When you setup configuration provider you can configure settings options represents by `SettingsProviderOptions`.

`SettingsProviderOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Cache | `bool` | Determines whether settings should be cached. |

Example configure configuration services:

```csharp
services.AddConfiguration()
    .AddJson<MySettings>("path_to_json_file", (serviceProvider, options) =>
    {
        options.Cache = true;
    });
```

## Usage

You can get Configuration in class constructor, for example, in Controller:

```csharp
using Structr.Configuration;

public class MyController : Controller
{
    private readonly MySettings _settings;

    public MyController(IConfiguration<MySettings> configuration)
    {
        _settings = configuration.Settings;
    }
    
    /* Actions */
}
```

Or elsewhere in the application using the `IServiceProvider`:

```csharp
var configuration = serviceProvider.GetRequiredService<IConfiguration<MySettings>>();
var settings = configuration.Settings;
```

If you want change settings in run-time, you can use `SetSettings()` method of `SettingsProvider` class, for example:

```csharp
settingsProvider.SetSettings(new MySettings { RootDirectory = "D:\\RootDirectory\\" });
```

Or you can use `Configure()` method of `Configurator` class, for example:

```csharp
var configurator = serviceProvider.GetRequiredService<IConfigurator<MySettings>>();
configurator.Configure(settings => settings.RootDirectory = "D:\\RootDirectory\\");
```