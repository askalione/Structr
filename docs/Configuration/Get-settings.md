# Get settings

`IConfiguration<>` is the main service to read settings.

Example: Inject `IConfiguration<>` into a application service:

```csharp
public class EmailService
{
    private readonly SmtpEmailSettings _settings;

    public NotificationService(IConfiguration<SmtpEmailSettings> configuration)
        => _settings = configuration.Settings;

    public Task SendEmailAsync(string email, string subject, string message)
    {
        using (var smtpClient = new SmtpClient(_settings.Host, _settings.Port))
        {
            /* Do send logic here */
        }
    }
}
```

Important thing that, in this example if you register `EmailService` as scoped, then private readonly field `_settings` that contains `SmtpEmailSettings` will not modified even origin `SmtpEmailSettings` will be changed. This will happen because you get settings `SmtpEmailSettings` from `IConfiguration<SmtpEmailSettings>` in the constructor.

So if you need get actual settings realtime, you should preserve `IConfiguration<SmtpEmailSettings>` in application service:

```csharp
public class EmailService
{
    private readonly IConfiguration<SmtpEmailSettings> _configuration;

    public EmailService(IConfiguration<SmtpEmailSettings> configuration)
        => _configuration = configuration;

    public Task SendEmailAsync(string email, string subject, string message)
    {
        var settings = _configuration.Settings; // Now, it is actual settings every `SendEmailAsync` invoke
        using (var smtpClient = new SmtpClient(settings.Host, settings.Port))
        {
            /* Do send logic here */
        }
    }
}
```