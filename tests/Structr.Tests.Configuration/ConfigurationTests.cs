using FluentAssertions;
using Structr.Configuration;
using Structr.Tests.Configuration.TestUtils;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("Tests with temp files")]
    public class ConfigurationTests
    {
        [Fact]
        public async Task Ctor()
        {
            // Arrange
            var provider = await TestDataManager.GetSettingsJsonProviderAsync(nameof(ConfigurationTests) + nameof(Ctor), true,
                ("FilePath", @"""X:\\readme.txt"""));
            var options = new ConfigurationOptions<TestSettings>(provider);

            // Act
            var result = new Configuration<TestSettings>(options);

            // Assert
            result.Settings.FilePath.Should().Be(@"X:\readme.txt");
        }

        [Fact]
        public void Ctor_throws_when_options_are_null()
        {
            // Act
            Action act = () => new Configuration<TestSettings>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
