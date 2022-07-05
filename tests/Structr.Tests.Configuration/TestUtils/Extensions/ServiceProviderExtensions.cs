using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Configuration;

namespace Structr.Tests.Configuration.TestUtils.Extensions
{
    internal static class ServiceProviderExtensions
    {
        public static void ShouldContainsConfigurationServices<TSettings>(this ServiceProvider serviceProvider)
            where TSettings : class, new()
        {
            serviceProvider.GetService<IConfiguration<TSettings>>().Should().NotBeNull();
            serviceProvider.GetService<IConfigurator<TSettings>>().Should().NotBeNull();
        }
    }
}
