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

Or you can use one of implemented settings provider from [list](Providers/Providers.md).

## Usage

Structr.Configuration provides separate services for the [get](Get-settings.md) and [set](Set-settings.md) settings.

Also you can [customize](Customization.md) the settings members with special attribute.