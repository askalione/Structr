# JSON-file settings provider

`JsonSettingsProvider` provides functionality for access to a JSON-file with settings.

## Setup

Create settings class.

```csharp
public class SmtpEmailSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
}
```

Create JSON file with settings:

```json
{
  "Host": "smtp.example.com",
  "Port": 25
}
```

Setup JSON-file settings provider:

```csharp
services.AddConfiguration()
    .AddJson<SmtpEmailSettings>("path_to_json_file");
```