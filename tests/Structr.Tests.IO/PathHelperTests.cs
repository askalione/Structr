using FluentAssertions;
using Structr.IO;
using Structr.Tests.IO.TestUtils;
using System;
using Xunit;

namespace Structr.Tests.IO
{
    public class PathHelperTests : IClassFixture<PathHelperFixture>
    {
        [Fact]
        public void Configure()
        {
            // Act
            PathHelper.Configure(options =>
            {
                options.Directories[ContentDirectory.Base] = ContentDirectoryDefaults.Base;
                options.Directories[ContentDirectory.Data] = ContentDirectoryDefaults.Data;
            });

            // Assert
            PathHelper.Options.Directories[ContentDirectory.Base].Should().Be(ContentDirectoryDefaults.Base);
            PathHelper.Options.Directories[ContentDirectory.Data].Should().Be(ContentDirectoryDefaults.Data);
        }

        [Fact]
        public void Configure_throws_when_options_are_null()
        {
            // Act
            Action act = () => PathHelper.Configure(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [ClassData(typeof(CombineTheoryData))]
        public void Combine(ContentDirectory directory, string path, string expected)
        {
            // Act
            var result = PathHelper.Combine(directory, path);

            // Assert
            result.Should().Be(expected);
        }

        private class CombineTheoryData : TheoryData<ContentDirectory, string, string>
        {
            public CombineTheoryData()
            {
                Add(ContentDirectory.Base, "readme.txt", ContentDirectoryDefaults.Base + "\\readme.txt");
                Add(ContentDirectory.Base, "dist\\readme.txt", ContentDirectoryDefaults.Base + "\\dist\\readme.txt");
                Add(ContentDirectory.Data, "readme.txt", ContentDirectoryDefaults.Data + "\\readme.txt");
                Add(ContentDirectory.Data, "dist\\readme.txt", ContentDirectoryDefaults.Data + "\\dist\\readme.txt");
            }
        }

        [Fact]
        public void Format()
        {
            // Arrange
            var path = @"|DataCustomDirectory|\foo\bar\baz.txt";
            PathHelper.Configure(options =>
            {
                options.Template = (directory) => $"|{directory}CustomDirectory|";
                options.Directories[ContentDirectory.Base] = ContentDirectoryDefaults.Base;
                options.Directories[ContentDirectory.Data] = ContentDirectoryDefaults.Data;
            });

            // Act
            var result = PathHelper.Format(path, ContentDirectory.Data);

            // Assert
            result.Should().Be(ContentDirectoryDefaults.Data + "\\foo\\bar\\baz.txt");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Format_throws_when_path_is_null_or_empty_with_whitespace(string path)
        {
            // Act
            Action act = () => PathHelper.Format(path);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Format_directory_array()
        {
            // Arrange
            var path = "|BaseCustomDirectory|\\|DataCustomDirectory|\\foo\\bar\\baz.txt";
            PathHelper.Configure(options =>
            {
                options.Template = (directory) => $"|{directory}CustomDirectory|";
                options.Directories[ContentDirectory.Base] = "D:";
                options.Directories[ContentDirectory.Data] = "Data";
            });

            // Act
            var result = PathHelper.Format(path, new ContentDirectory[] { ContentDirectory.Base, ContentDirectory.Data });

            // Assert
            result.Should().Be("D:\\Data\\foo\\bar\\baz.txt");
        }

        [Fact]
        public void Format_throws_when_directory_array_is_null()
        {
            // Act
            Action act = () => PathHelper.Format("\\foo\\bar\\baz.txt", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
