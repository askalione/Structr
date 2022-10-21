using Consul;
using Microsoft.Extensions.DependencyInjection;
using Structr.Samples;
using Structr.Samples.Configuration.Consul;
using Structr.Samples.Configuration.Consul.Settings;

var services = new ServiceCollection();

// Add sample app
services.AddSample<App>();

// Add Configuration
services.AddConfiguration()
    .AddConsul<AppSettings>("AppSettings", new ConsulClient());

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<IApp>();

await app.RunAsync();