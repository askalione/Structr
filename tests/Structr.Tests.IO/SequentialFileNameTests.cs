using FluentAssertions;
using Structr.IO;
using System;
using Xunit;

namespace Structr.Tests.IO
{
    public class SequentialFileNameTests
    {
        [Fact]
        public void NewFileName()
        {
            // Act
            var result = SequentialFileName.NewFileName();

            // Assert
            result.Should().HaveLength(47);
        }

        [Fact]
        public void NewFileName_generates_new_value_each_run()
        {
            // Arrange
            var firstRunResult = SequentialFileName.NewFileName();

            // Act
            var secondRunResult = SequentialFileName.NewFileName();

            // Assert
            secondRunResult.Should().NotBe(firstRunResult);
        }

        [Fact]
        public void NewFileName_from_existing_file_name()
        {
            // Act
            var result = SequentialFileName.NewFileName("readme.txt");

            // Assert
            result.Should().HaveLength(51)
                .And.EndWith(".txt");
        }

        [Fact]
        public void NewFileNameWithMimeType()
        {
            // Act
            var result = SequentialFileName.NewFileNameWithMimeType("text/xml");

            // Assert
            result.Should().HaveLength(51)
                .And.EndWith(".xml");
        }

        [Fact]
        public void NewFileNameWithExtension()
        {
            // Act
            var result = SequentialFileName.NewFileNameWithExtension(".csv");

            // Assert
            result.Should().HaveLength(51)
                .And.EndWith(".csv");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void NewFileNameWithExtension_throws_when_extension_is_null_or_empty_with_whitespace(string extension)
        {
            // Act
            Action act = () => SequentialFileName.NewFileNameWithExtension(extension);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
