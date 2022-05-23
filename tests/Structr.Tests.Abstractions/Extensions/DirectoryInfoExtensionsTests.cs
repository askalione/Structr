using FluentAssertions;
using Structr.Abstractions.Extensions;
using Structr.Tests.Abstractions.TestsUtils;
using System;
using System.IO;
using Xunit;

namespace Structr.Tests.Abstractions.Extensions
{
    [Collection("FileSystemTests")]
    public class DirectoryInfoExtensionsTests : IDisposable
    {
        private readonly string _path;

        public DirectoryInfoExtensionsTests()
        {
            _path = FileSystemTestFixture.GetTestingPath(this.GetType());
            Directory.CreateDirectory(_path);
        }

        public void Dispose()
        {
            Directory.Delete(_path, true);
        }

        [Fact]
        public void GetParent()
        {
            // Arrange
            CreateDirectoriesCascade(_path, "1", "2", "3", "4");
            var directoryInfo = new DirectoryInfo(Path.Combine(_path, "1", "2", "3"));

            // Act
            var result = directoryInfo.GetParent(2);

            // Assert
            result.FullName.Should().Be(Path.Combine(_path, "1"));
        }

        [Fact]
        public void GetParent_throws_when_level_less_than_1()
        {
            // Arrange
            CreateDirectoriesCascade(_path, "1", "2", "3", "4");
            var directoryInfo = new DirectoryInfo(Path.Combine(_path, "1", "2", "3"));

            // Act
            Action act = () => directoryInfo.GetParent(0);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Level must be greater or equal 1*level*");
        }

        private void CreateDirectoriesCascade(string startingPath, params string[] names)
        {
            var currentPath = startingPath;
            foreach (var name in names)
            {
                currentPath = Path.Combine(currentPath, name);
                Directory.CreateDirectory(currentPath);
            }
        }
    }
}