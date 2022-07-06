# Razor

You can use Razor templates instead of default templates ("{{" and "}}") for email notification content.

## Installation

Email.Razor package is available on [NuGet](https://www.nuget.org/packages/Structr.Email.Razor/).

```
dotnet add package Structr.Email.Razor
```

## Setup

Setup services:

```csharp
services.AddEmail("from@example.com", options => {
    options.TemplateRootPath = "C:\\Templates"; // Important to configure this option when using email templates.
})
    .AddSmtpClient(host: "127.0.0.1", port: 25)
    .AddRazorTemplateRenderer();
```

**Important**: Be sure that you configured `TemplateRootPath` option. It's an absolute path to root directory with email notification templates.

Example of using static template file:

```csharp
// Define Model class for template.
class Recipient
{
    public string Name { get; set; }
}

// Define static template file
class RecipientEmailTemplateFileMessage : EmailTemplateMessage<Recipient>
{
    public override string TemplatePath => "RecipientTemplate.cshtml"; // Absolute path is "C:\Templates\RecipientTemplate.cshtml".

    public RecipientEmailTemplateFileMessage(string to, Recipient model) 
        : base(new EmailAddress(to), model)
    {}
}

// Create instance of Model class.
var model = new Recipient { Name = "Peter Parker" };

// Create message
var message = new RecipientEmailTemplateFileMessage("to@example.com", model);

// Send email notification generated via template from "C:\Templates\RecipientTemplate.cshtml".
await _emailSender.SendEmailAsync(message);
```