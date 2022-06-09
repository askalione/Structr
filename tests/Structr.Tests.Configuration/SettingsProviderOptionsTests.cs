using Xunit;
using FluentAssertions;
using Structr.Configuration;

namespace Structr.Tests.Configuration
{
    public class SettingsProviderOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new SettingsProviderOptions();

            // Assert
            result.Cache.Should().BeTrue();
        }
    }
}
