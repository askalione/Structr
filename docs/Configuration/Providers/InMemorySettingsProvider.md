# In-Memory settings provider

`InMemorySettingsProvider` provides functionality for access to in memory settings.

## Setup

Create settings class.

```csharp
public class SmtpEmailSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
}
```

Setup In-Memory configuration provider:

```csharp
services.AddConfiguration()
    .AddInMemory<SmtpEmailSettings>(new SmtpEmailSettings {
        Host = "smtp.example.com",
        Post = 25
    });
```