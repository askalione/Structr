# Configuration

**Structr.Configuration** package is intended to simplify work with application settings via mapping to classes to provide strongly typed access to groups of related settings. 

## Installation

Configuration package is available on [NuGet](https://www.nuget.org/packages/Structr.Configuration/).

```
dotnet add package Structr.Configuration
```

## Setup

Create settings class.

```csharp
public class SmtpEmailSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
}
```

Configuration services uses different providers to get source settings. For example: JSON and XML file, Database, Consul KV, or something else.

You can create custom settings provider:

```csharp
public class CustomConfigurationProvider<TSettings> : SettingsProvider<TSettings>
    where TSettings : class, new()
{
    public CustomConfigurationProvider(SettingsProviderOptions options) 
        : base(options) 
    {}
    
    protected override TSettings LoadSettings()
    {
        /* Do some logic here */
    }

    protected override void UpdateSettings(TSettings settings)
    {
        /* Do some logic here */
    }

    protected override bool IsSettingsModified()
    {
        /* Do some logic here */
    }

    protected override void LogFirstAccess()
    { 
        /* Do some logic here */
    }
}
```

And then setup configuration services:

```csharp
services.AddConfiguration()
    .AddProvider(new CustomConfigurationProvider<SmtpEmailSettings>(new SettingsProviderOptions()));
```

Or you can use one of default implemented settings provider from list:

| Provider name | NuGet package |
| --- | --- |
| [JSON-file](Providers/JsonSettingsProvider.md) | [Structr.Configuration](https://www.nuget.org/packages/Structr.Configuration/) |
| [XML-file](Providers/XmlSettingsProvider.md) | [Structr.Configuration](https://www.nuget.org/packages/Structr.Configuration/) |
| [In-Memory](Providers/InMemorySettingsProvider.md) | [Structr.Configuration](https://www.nuget.org/packages/Structr.Configuration/) |
| [Consul](Providers/ConsulSettingsProvider.md) | [Structr.Configuration.Consul](https://www.nuget.org/packages/Structr.Configuration.Consul/) |

### Settings provider options

When you setup settings provider you can configure provider options represents by `SettingsProviderOptions`.

`SettingsProviderOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Cache | `bool` | Determines whether settings should be cached, `true` by default. |

Example configure services:

```csharp
services.AddConfiguration()
    .AddJson<SmtpEmailSettings>("path_to_json_file", (serviceProvider, options) =>
    {
        options.Cache = true;
    });
```

If `Cache` options was setting up to `true` then settings provider return cached settings while `IsSettingsModified()` returns `false`, otherwise every settings request invoke `LoadSettings()` method. 

## Usage

Structr.Configuration provides separate services for the [get](Get-settings.md) and [set](Set-settings.md) settings.

Also you can [customize](Customization.md) the settings members with special attribute.