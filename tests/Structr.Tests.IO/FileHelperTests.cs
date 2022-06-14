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
        public FileHelperTests()
        {
            FileHelperFixture.DeleteTestFiles();
        }

        [Fact]
        public void SaveFile()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            var filePath = FileHelper.SaveFile(TestDataPath.Path, bytes);

            // Assert
            filePath.Should().Be(TestDataPath.Path);
            File.Exists(filePath).Should().BeTrue();
            File.ReadAllText(filePath).Should().Be(_text);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SaveFile_throws_when_path_is_null_or_empty_with_whitespace(string path)
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            Action act = () => FileHelper.SaveFile(path, bytes);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void SaveFile_throws_when_bytes_is_null()
        {
            // Act
            Action act = () => FileHelper.SaveFile(TestDataPath.Path, null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void SaveFile_creates_directory_if_not_exist()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            var filePath = FileHelper.SaveFile(TestDataPath.NonExistentPath, bytes, true);

            // Assert
            filePath.Should().Be(TestDataPath.NonExistentPath);
            File.Exists(filePath).Should().BeTrue();
            File.ReadAllText(filePath).Should().Be(_text);
        }

        [Fact]
        public void SaveFile_throws_if_directory_not_exist()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            Action act = () => FileHelper.SaveFile(TestDataPath.NonExistentPath, bytes, false);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void SaveFile_creates_file_with_sequential_name_if_exists()
        {
            // Arrange
            AddTestFile();
            byte[] bytes = Encoding.UTF8.GetBytes("Some new text");

            // Act
            var filePath = FileHelper.SaveFile(TestDataPath.Path, bytes,
                useSequentialFileNameIfExists: true);

            // Assert
            filePath.Should().Be(TestDataPath.NextUniquePath);
            File.Exists(filePath).Should().BeTrue();
            File.ReadAllText(filePath).Should().Be("Some new text");
        }

        [Fact]
        public void SaveFile_throws_if_file_already_exist()
        {
            // Arrange
            AddTestFile();
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            Action act = () => FileHelper.SaveFile(TestDataPath.Path, bytes,
                useSequentialFileNameIfExists: false);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public async Task SaveFileAsync()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            var filePath = await FileHelper.SaveFileAsync(TestDataPath.Path, bytes);

            // Assert
            filePath.Should().Be(TestDataPath.Path);
            File.Exists(filePath).Should().BeTrue();
            File.ReadAllText(filePath).Should().Be(_text);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task SaveFileAsync_throws_when_path_is_null_or_empty_with_whitespace(string path)
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            Func<Task> act = async () => await FileHelper.SaveFileAsync(path, bytes);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SaveFileAsync_throws_when_bytes_is_null()
        {
            // Act
            Func<Task> act = async () => await FileHelper.SaveFileAsync(TestDataPath.Path, null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SaveFileAsync_creates_directory_if_not_exist()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            var filePath = await FileHelper.SaveFileAsync(TestDataPath.NonExistentPath, bytes, true);

            // Assert
            filePath.Should().Be(TestDataPath.NonExistentPath);
            File.Exists(filePath).Should().BeTrue();
            File.ReadAllText(filePath).Should().Be(_text);
        }

        [Fact]
        public async Task SaveFileAsync_throws_if_directory_not_exist()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            Func<Task> act = async () => await FileHelper.SaveFileAsync(TestDataPath.NonExistentPath, bytes, false);

            // Assert
            await act.Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task SaveFileAsync_creates_file_with_sequential_name_if_exists()
        {
            // Arrange
            AddTestFile();
            byte[] bytes = Encoding.UTF8.GetBytes("Some new text");

            // Act
            var filePath = await FileHelper.SaveFileAsync(TestDataPath.Path, bytes,
                useSequentialFileNameIfExists: true);

            // Assert
            filePath.Should().Be(TestDataPath.NextUniquePath);
            File.Exists(TestDataPath.NextUniquePath).Should().BeTrue();
            File.ReadAllText(TestDataPath.NextUniquePath).Should().Be("Some new text");
        }

        [Fact]
        public async Task SaveFileAsync_throws_if_file_already_exist()
        {
            // Arrange
            AddTestFile();
            byte[] bytes = Encoding.UTF8.GetBytes(_text);

            // Act
            Func<Task> act = async () => await FileHelper.SaveFileAsync(TestDataPath.Path, bytes,
                useSequentialFileNameIfExists: false);

            // Assert
            await act.Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Fact]
        public void ReadFile()
        {
            // Arrange
            AddTestFile();

            // Act
            var result = FileHelper.ReadFile(TestDataPath.Path);

            // Assert
            Encoding.UTF8.GetString(result).Should().Be(_text);
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
            var result = FileHelper.ReadFile(TestDataPath.NonExistentPath, false);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ReadFileAsync()
        {
            // Arrange
            AddTestFile();

            // Act
            var result = await FileHelper.ReadFileAsync(TestDataPath.Path);

            // Assert
            Encoding.UTF8.GetString(result).Should().Be(_text);
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
            var result = await FileHelper.ReadFileAsync(TestDataPath.NonExistentPath, false);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadFile_from_stream()
        {
            // Arrange
            Stream stream = new MemoryStream();
            byte[] bytes = Encoding.UTF8.GetBytes(_text);
            foreach (var @byte in bytes)
            {
                stream.WriteByte(@byte);
            }

            // Act
            var result = FileHelper.ReadFile(stream);

            // Assert
            Encoding.UTF8.GetString(result).Should().Be(_text);
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
            byte[] bytes = Encoding.UTF8.GetBytes(_text);
            foreach (var @byte in bytes)
            {
                stream.WriteByte(@byte);
            }

            // Act
            var result = await FileHelper.ReadFileAsync(stream);

            // Assert
            Encoding.UTF8.GetString(result).Should().Be(_text);
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
            // Arrange
            AddTestFile();

            // Act
            FileHelper.DeleteFile(TestDataPath.Path);

            // Assert
            File.Exists(TestDataPath.Path).Should().BeFalse();
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
        public void GetFilePathWithSequentialFileName()
        {
            // Arrange
            AddTestFile();

            // Act
            var result = FileHelper.GetFilePathWithSequentialFileName(TestDataPath.Path);

            // Assert
            result.Should().Be(TestDataPath.NextUniquePath);
        }

        private const string _text = "Hello world!";

        private static void AddTestFile(string text = _text)
            => File.WriteAllText(TestDataPath.Path, text);
    }
}
