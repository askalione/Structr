using FluentAssertions;
using Structr.IO;
using Structr.Tests.IO.TestUtils;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.IO
{
    public class FileHelperTests : IClassFixture<FileHelperFixture>
    {
        FileHelperFixture _fixture;

        public FileHelperTests(FileHelperFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void SaveFile()
        {
            // Arrange
            if (File.Exists(_fixture.Path))
            {
                File.Delete(_fixture.Path);
            }
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            var filePath = FileHelper.SaveFile(_fixture.Path, bytes, true, true);

            // Assert
            filePath.Should().Be(_fixture.Path);
            File.Exists(filePath).Should().BeTrue();
            File.ReadAllText(filePath).Should().Be(_fixture.Text);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveFile_throws_when_path_is_null_or_empty_with_whitespace(string path)
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            Action act = () => FileHelper.SaveFile(path, bytes);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void SaveFile_throws_when_bytes_is_null()
        {
            // Act
            Action act = () => FileHelper.SaveFile(_fixture.Path, null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void SaveFile_throws_if_file_already_exist()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            Action act = () => FileHelper.SaveFile(_fixture.Path, bytes);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void SaveFile_throws_if_directory_not_exist()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            Action act = () => FileHelper.SaveFile(_fixture.NonExistentPath, bytes, false);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void SaveFile_with_unique_file_name()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            var filePath = FileHelper.SaveFile(_fixture.Path, bytes, true, true);

            // Assert
            filePath.Should().Be(_fixture.NextUniquePath);
            File.Exists(_fixture.NextUniquePath).Should().BeTrue();
            File.ReadAllText(_fixture.NextUniquePath).Should().Be(_fixture.Text);
            if (File.Exists(_fixture.NextUniquePath))
            {
                File.Delete(_fixture.NextUniquePath);
            }
        }

        [Fact]
        public async Task SaveFileAsync()
        {
            // Arrange
            if (File.Exists(_fixture.Path))
            {
                File.Delete(_fixture.Path);
            }
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            var filePath = await FileHelper.SaveFileAsync(_fixture.Path, bytes, true, true);

            // Assert
            filePath.Should().Be(_fixture.Path);
            File.Exists(filePath).Should().BeTrue();
            File.ReadAllText(filePath).Should().Be(_fixture.Text);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task SaveFileAsync_throws_when_path_is_null_or_empty_with_whitespace(string path)
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            Func<Task> act = async () => await FileHelper.SaveFileAsync(path, bytes);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SaveFileAsync_throws_when_bytes_is_null()
        {
            // Act
            Func<Task> act = async () => await FileHelper.SaveFileAsync(_fixture.Path, null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SaveFileAsync_throws_if_file_already_exist()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            Func<Task> act = async () => await FileHelper.SaveFileAsync(_fixture.Path, bytes);

            // Assert
            await act.Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task SaveFileAsync_throws_if_directory_not_exist()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            Func<Task> act = async () => await FileHelper.SaveFileAsync(_fixture.NonExistentPath, bytes, false);

            // Assert
            await act.Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task SaveFileAsync_with_unique_file_name()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);

            // Act
            var filePath = await FileHelper.SaveFileAsync(_fixture.Path, bytes, true, true);

            // Assert
            filePath.Should().Be(_fixture.NextUniquePath);
            File.Exists(_fixture.NextUniquePath).Should().BeTrue();
            File.ReadAllText(_fixture.NextUniquePath).Should().Be(_fixture.Text);
            if (File.Exists(_fixture.NextUniquePath))
            {
                File.Delete(_fixture.NextUniquePath);
            }
        }

        [Fact]
        public void ReadFile()
        {
            // Act
            var result = FileHelper.ReadFile(_fixture.Path);

            // Assert
            Encoding.UTF8.GetString(result).Should().Be(_fixture.Text);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ReadFile_throws_when_path_is_null_or_empty_with_whitespace(string path)
        {
            // Act
            Action act = () => FileHelper.ReadFile(path);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ReadFile_file_not_exist()
        {
            // Act
            var result = FileHelper.ReadFile(_fixture.NonExistentPath, false);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ReadFileAsync()
        {
            // Act
            var result = await FileHelper.ReadFileAsync(_fixture.Path);

            // Assert
            Encoding.UTF8.GetString(result).Should().Be(_fixture.Text);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ReadFileAsync_throws_when_path_is_null_or_empty_with_whitespace(string path)
        {
            // Act
            Func<Task> act = async () => await FileHelper.ReadFileAsync(path);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ReadFileAsync_file_not_exist()
        {
            // Act
            var result = await FileHelper.ReadFileAsync(_fixture.NonExistentPath, false);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadFile_from_stream()
        {
            // Arrange
            Stream stream = new MemoryStream();
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);
            foreach (var @byte in bytes)
            {
                stream.WriteByte(@byte);
            }

            // Act
            var result = FileHelper.ReadFile(stream);

            // Assert
            Encoding.UTF8.GetString(result).Should().Be(_fixture.Text);
        }

        [Fact]
        public void ReadFile_throws_when_stream_is_null()
        {
            // Act
            Action act = () => FileHelper.ReadFile(null, 0);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task ReadFileAsync_from_stream()
        {
            // Arrange
            Stream stream = new MemoryStream();
            byte[] bytes = Encoding.UTF8.GetBytes(_fixture.Text);
            foreach (var @byte in bytes)
            {
                stream.WriteByte(@byte);
            }

            // Act
            var result = await FileHelper.ReadFileAsync(stream);

            // Assert
            Encoding.UTF8.GetString(result).Should().Be(_fixture.Text);
        }

        [Fact]
        public async Task ReadFileAsync_throws_when_stream_is_null()
        {
            // Act
            Func<Task> act = async () => await FileHelper.ReadFileAsync(null, 0);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public void DeleteFile()
        {
            // Act
            FileHelper.DeleteFile(_fixture.Path);

            // Assert
            File.Exists(_fixture.Path).Should().BeFalse();
            File.WriteAllText(_fixture.Path, _fixture.Text);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void DeleteFile_throws_when_path_is_null_or_empty_with_whitespace(string path)
        {
            // Act
            Action act = () => FileHelper.DeleteFile(path);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void GetFilePathWithUniqueFileName()
        {
            // Act
            var result = FileHelper.GetFilePathWithUniqueFileName(_fixture.Path);

            // Assert
            result.Should().Be(_fixture.NextUniquePath);
        }
    }
}
