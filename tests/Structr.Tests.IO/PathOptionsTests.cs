using FluentAssertions;
using Structr.IO;
using Xunit;

namespace Structr.Tests.IO
{
    public class PathOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new PathOptions();

            // Assert
            result.Template(ContentDirectory.Base).Should().Be("|BaseDirectory|");
            result.Template(ContentDirectory.Data).Should().Be("|DataDirectory|");
            result.Directories.Should().ContainKeys(ContentDirectory.Base, ContentDirectory.Data);
        }
    }
}
