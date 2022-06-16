# Email

**Structr.Email** package is intended to help send emails.

## Installation

Email package is available on [NuGet](https://www.nuget.org/packages/Structr.Email/).

```
dotnet add package Structr.Email
```

## Setup

To use the Email component setup email services. You can use SMTP-client to send emails.

```csharp
EmailServiceBuilder emailServiceBuilder = services.AddEmail(new EmailAddress("from@example.com"))
    .AddSmtpClient(host: "127.0.0.1", port: 25);
```

Or you can use File-client, in development environment, for example.

```csharp
EmailServiceBuilder emailServiceBuilder = services.AddEmail(new EmailAddress("from@example.com"))
    .AddFileClient(/*path to the directory with an emails*/);
```

## Usage

After setup you can use `IEmailSender` to send emails in your application:

```csharp
public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
{
    private readonly IEmailSender _emailSender;

    public SendEmailCommandHandler(IEmailSender emailSender)
        => _emailSender = emailSender;

    public async Task HandleAsync(SendEmailCommand command)
    {
        // command.To: "to@example.com"
        // command.Message: "Some email message."
        // command.Subject: "Some email subject."
        // command.AttachmentFilePath: "C:/Attachments/Attachment.txt"
        await _emailSender.SendEmailAsync(new EmailMessage(command.To, command.Message)
        {
            Subject = command.Subject,
            Attachments = new[] {
                new EmailAttachment(command.AttachmentFilePath, "text/plain")
            }
        });
    }
}
```

### Email content from template

You can use `EmailTemplateMessage`, `EmailTemplateMessage<TModel>`, `EmailTemplateFileMessage` or `EmailTemplateFileMessage<TModel>` to send email generated from template.

```csharp
  // Create your Model class for template
  class CustomModel
  {
      public string Name { get; set; }
  }

  // Create instance of your Model class
  var model = new CustomModel { Name = "Peter Parker" };

  // Create message
  var message = new EmailTemplateMessage("to@example.com", "Hello, {{Name}}!", model);

  // Send email with text "Hello, Peter Parker!"
  await _emailSender.SendEmailAsync(message);
```

## Razor email template rendering

You can use Razor templates for email content. For this install **Structr.Email.Razor** package from [NuGet](https://www.nuget.org/packages/Structr.Email.Razor/).

```
dotnet add package Structr.Email.Razor
```

And then setup Email component with `.AddRazorTemplateRenderer()`.

```csharp
EmailServiceBuilder emailServiceBuilder = services.AddEmail(new EmailAddress("from@example.com"))
    .AddSmtpClient(host: "127.0.0.1", port: 25)
    .AddRazorTemplateRenderer();
```