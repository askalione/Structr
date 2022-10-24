# Settings providers

List of implemented settings providers:

| Provider name | NuGet package |
| --- | --- |
| [JSON-file](JsonSettingsProvider.md) | [Structr.Configuration](https://www.nuget.org/packages/Structr.Configuration/) |
| [XML-file](XmlSettingsProvider.md) | [Structr.Configuration](https://www.nuget.org/packages/Structr.Configuration/) |
| [In-Memory](InMemorySettingsProvider.md) | [Structr.Configuration](https://www.nuget.org/packages/Structr.Configuration/) |
| [Consul](ConsulSettingsProvider.md) | [Structr.Configuration.Consul](https://www.nuget.org/packages/Structr.Configuration.Consul/) |

## Settings provider options

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
