using FluentAssertions;

namespace Structr.Tests.Configuration.TestUtils.Extensions
{
    internal static class TestSettingsExtensions
    {
        public static void ShouldBeEquivalentToExpectedSettings(this TestSettings settings)
        {
            var expected = new TestSettings
            {
                FilePath = "D:\\readme.txt"
            };

            settings.Should().BeEquivalentTo(expected);
        }
    }
}
