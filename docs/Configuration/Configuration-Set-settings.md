# Set settings

`IConfigurator<>` is the main service to write settings.

Example: Inject `IConfigurator<>` into a command handler:

```csharp
public class EditEmailSettingsCommandHandler : ICommandHandler<EditEmailSettingsCommand>
{
    private readonly IConfigurator<SmtpEmailSettings> _configurator;

    public EditEmailSettingsCommandHandler(IConfigurator<SmtpEmailSettings> configurator)
        => _configurator = configurator;

    public Task HandleAsync(EditEmailSettingsCommand command)
    {
        // Configure settings by command data.
        _configurator.Configure(settings =>
        {
            settings.Host = command.Host;
            settings.Port = command.Port;
        });
    }
}
```

Simple settings modification mechanism out of the box is one of the most distinguishing features from the Microsoft [Options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options).