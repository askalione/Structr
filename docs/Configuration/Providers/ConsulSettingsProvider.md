# Consul settings provider

`ConsulSettingsProvider` provides functionality of storing settings in Consul KV store.

## Installation

Consul settings provider package is available on [NuGet](https://www.nuget.org/packages/Structr.Configuration.Consul/).

```
dotnet add package Structr.Configuration.Consul
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

Setup Consul settings provider:

```csharp
services.AddConfiguration()
    .AddConsul<SmtpEmailSettings>("consul_key", new ConsulClient(options => {
        options.Address = new Uri("http://localhost:8500"); // Base URL to Consul KV store.
    }));
```
