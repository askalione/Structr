using Microsoft.Extensions.DependencyInjection;
using Structr.Email;
using Structr.Samples;
using Structr.Samples.Email;

var services = new ServiceCollection();

// Add sample app
services.AddSample<App>();

var path = AppHelper.GetRootPath("Templates");

// Add email
var emailBuilder = services.AddEmail(new EmailAddress("from@example.com", "Example"), options =>
{
    options.TemplateRootPath = AppHelper.GetRootPath("Templates");
})
    // Use SMTP-client
    //.AddSmtpClient(host: "127.0.0.1", port: 25);
    // Or use File-client, in development environment, for example
    .AddFileClient(AppHelper.GetExecutablePath("Emails"));

#if RAZOR
// Optional: Use Razor templates.
emailBuilder.AddRazorTemplateRenderer();
#endif

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<IApp>();

await app.RunAsync();