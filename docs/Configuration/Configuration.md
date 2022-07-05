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

Configuration services uses different providers to get source settings. For example: JSON, XML file, Database, or something else.

You can create custom configuration provider:

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
}
```

And then setup configuration services:

```csharp
services.AddConfiguration()
    .AddProvider(new CustomConfigurationProvider<SmtpEmailSettings>());
```

Or you can use one of default implemented configuration provider from list:

* [JSON](#json-provider)
* [XML](#xml-provider)

### JSON provider

Create JSON file with settings:

```json
{
  "Host": "smtp.example.com",
  "Port": 25
}
```

Setup JSON configuration provider:

```csharp
services.AddConfiguration()
    .AddJson<SmtpEmailSettings>("path_to_json_file");
```

### XML provider

Create XML file with settings: 

```xml
<?xml version="1.0" encoding="utf-16"?>
<SmtpEmailSettings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Host>smtp.example.com</Host>
  <Port>25</Port>
</SmtpEmailSettings>
```

Setup XML configuration provider:

```csharp
services.AddConfiguration()
    .AddXml<SmtpEmailSettings>("path_to_xml_file");
```

### Options

When you setup configuration provider you can configure provider options represents by `SettingsProviderOptions`.

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

Structr.Configuration provides separate services for the [get](/Configuration-Read-settings.md) and [set](/Configuration-Write-settings.md) settings.

Also you can [customize](/Configuration-Customization.md) the settings members with special attribute.