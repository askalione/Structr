# Email

**Structr.Email** package is intended to help sending email notifications.

## Installation

Email package is available on [NuGet](https://www.nuget.org/packages/Structr.Email/).

```
dotnet add package Structr.Email
```

## Setup

Setup basic email service:

```csharp
// Configure email address.
services.AddEmail("from@example.com")
    /* Setup one of email clients here. */;

// Configure email address of a sender with name and email templates root directory path.
services.AddEmail(new EmailAddress("from@example.com", "Example"), options => {
    options.TemplateRootPath = "C:\\Templates";  
})
    /* Setup one of email clients here. */;
```

`AddEmail()` extension method performs registration email service `IEmailSender`. Additionally configure `IEmailSender` service used `EmailOptions` class.
`EmailOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| From | `EmailAddress` | Email address of a sender. Required option, which configuring with `AddEmail()` method. | 
| TemplateRootPath | `string?` | Root directory path with email templates (needed if using email templates), default value is `null`. |

Email service uses different email clients. For example: File-client, SMTP-client, clients for cloud-based email delivery platform (like SendGrid, MailChimp etc.) or something else.
You can use one of default implemented email client from list:

* [SMTP-client](#smtp-client)
* [File-client](#file-client)

### SMTP-client

An SMTP-client allows sending of email notifications using a SMTP server.
Setup email service with SMTP-client:

```csharp
services.AddEmail("from@example.com")
    .AddSmtpClient(host: "127.0.0.1", port: 25, options => {
        options.User = "user";
        options.Password = "password";
        options.IsSslEnabled = true;
    });
```

Additionally configure SMTP-client used `SmtpOptions` class.
`SmtpOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Host | `string` | Domain name or IP address (IPv4) of the host used for SMTP transactions. Required option, which configuring with `AddSmtpClient()` method. | 
| Port | `int` | Port used for SMTP transactions. Required option, which configuring with `AddSmtpClient()` method, default value is `25`. |
| User | `string?` | User name used to authenticate a sender, default value is `null`. |
| Password | `string?` | User password used to authenticate a sender, default value is `null`. |
| IsSslEnabled | `bool` | Specify whether the SMTP-client uses SSL to encrypt a connection, default value is `false`. |

### File-client

An File-client allows preserving of email notifications to file system. This behavior can be useful in development environment for example.
Setup email service with File-client:

```csharp
services.AddEmail("from@example.com")
    .AddFileClient("C:\\Emails"); // All sending emails preserves in "C:\\Emails" directory.
```

You also can use clients depending on the environment. For example you can use File-client for development environment and SMTP-client for production environment.

```csharp
EmailServiceBuilder builder = services.AddEmail("from@example.com");
#if DEBUG
    builder.AddFileClient("C:\\Emails");
#else
    builder.AddSmtpClient(host: "127.0.0.1", port: 25);
#endif
```

## Usage

After setup you can inject `IEmailSender` service and use it for sending email notifications:

```csharp
public class ReportService : IReportService
{
    private readonly IDbContext _dbContext;
    private readonly IEmailSender _emailSender;

    public ReportService(IDbContext dbContext, IEmailSender emailSender)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
    }

    public async Task<bool> SendReportAsync(int userId, string subject, string message, string reportFilePath)
    {
        var to = await _dbContext.Users
            .Where(x => x.Id == userId)
            .Select(x => x.Email)
            .SingleOrDefaultAsync();

        var emailMessage = new EmailMessage(to, message)
        {
            Subject = subject,
            Attachments = new[] {
                new EmailAttachment(reportFilePath, "text/html")
            }
        };

        return await _emailSender.SendEmailAsync(emailMessage);
    }
}
```

### Templates

You can use `EmailTemplateMessage`, `EmailTemplateFileMessage` classes to sending email notifications generating via templates.
By default templates constructing via "{{" and "}}", for example "{{UserName}}".

Example of using dynamic template:

```csharp
// Define Model class for template.
class Recipient
{
    public string Name { get; set; }
}

// Create instance of Model class.
var model = new Recipient { Name = "Peter Parker" };

// Create message
var message = new EmailTemplateMessage("to@example.com", "Hello, {{Name}}!", model);

// Send email notification with text "Hello, Peter Parker!".
await _emailSender.SendEmailAsync(message);
```

Example of using static template:

```csharp
// Define Model class for template.
class Recipient
{
    public string Name { get; set; }
}

// Define static template
class RecipientEmailTemplateMessage : EmailTemplateMessage<Recipient>
{
    public override string Template => "Hello, {{Name}}!";

    public RecipientEmailTemplateMessage(string to, Recipient model) 
        : base(new EmailAddress(to), model)
    {}
}

// Create instance of Model class.
var model = new Recipient { Name = "Peter Parker" };

// Create message
var message = new RecipientEmailTemplateMessage("to@example.com", model);

// Send email notification with text "Hello, Peter Parker!".
await _emailSender.SendEmailAsync(message);
```

See example of using template file in [Razor](Razor.md) section.