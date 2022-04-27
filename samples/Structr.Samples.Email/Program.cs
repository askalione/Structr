using Microsoft.Extensions.DependencyInjection;
using Structr.Email;
using Structr.Samples;
using Structr.Samples.Email;

var services = new ServiceCollection();

// Add sample app
services.AddSample<App>();

// Add email
// Option 1: Use SMTP-client
//services.AddEmail(new EmailAddress("from@example.com", "Example"))
//    .AddSmtpClient(host: "127.0.0.1", port: 25);

// Option 2: Use File-client, in development environment, for example
services.AddEmail(new EmailAddress("from@example.com", "Example"))
    .AddFileClient(Path.Combine(Directory.GetCurrentDirectory(), "Emails"));

// TODO: Razor

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<IApp>();

await app.RunAsync();