using FluentAssertions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using Structr.Tests.Configuration.TestUtils.Extensions;
using System;
using Xunit;

namespace Structr.Tests.Configuration
{
    public class ConfigurationTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");
            var provider = new JsonSettingsProvider<TestSettings>(new SettingsProviderOptions(), path);
            var options = new ConfigurationOptions<TestSettings>(provider);

            // Act
            var result = new Configuration<TestSettings>(options);

            // Assert
            result.Settings.ShouldBeEquivalentToExpectedSettings();
        }

        [Fact]
        public void Ctor_throws_ArgumentNullException_if_options_are_null()
        {
            // Act
            Action act = () => new Configuration<TestSettings>(null); ;

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'options')");
        }
    }
}
