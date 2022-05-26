using FluentAssertions;
using Structr.IO;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.IO
{
    public class MimeTypeHelperTests
    {
        [Theory]
        [InlineData(".323", "text/h323")]
        [InlineData(".aiff", "audio/aiff")]
        [InlineData(".bcpio", "application/x-bcpio")]
        [InlineData(".c", "text/plain")]
        [InlineData(".csproj", "text/plain")]
        [InlineData(".css", "text/css")]
        [InlineData(".csv", "text/csv")]
        [InlineData(".hdml", "text/x-hdml")]
        [InlineData(".ico", "image/x-icon")]
        [InlineData(".odl", "text/plain")]
        [InlineData(".psess", "application/xml")]
        [InlineData("svg", "image/svg+xml")]
        [InlineData(".xrm-ms", "text/xml")]
        [InlineData(".mytestfileextension", "application/octet-stream")]
        [InlineData("fictitiousfileextension", "application/octet-stream")]
        [InlineData(".aaa", "application/octet-stream")]
        public void GetMimeType(string extension, string expected)
        {
            // Act
            var result = MimeTypeHelper.GetMimeType(extension);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void GetMimeType_throws_when_extension_is_null_or_empty_with_whitespace(string extension)
        {
            // Act
            Action act = () => MimeTypeHelper.GetMimeType(extension);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData("application/octet-stream", ".bin")]
        [InlineData("application/vnd.ms-excel", ".xls")]
        [InlineData("application/x-zip-compressed", ".zip")]
        [InlineData("image/pict", ".pic")]
        [InlineData("video/mp4", ".mp4")]
        public void GetExtension(string mimeType, string expected)
        {
            // Act
            var result = MimeTypeHelper.GetExtension(mimeType);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void GetExtension_throws_when_mimeType_is_null_or_empty_with_whitespace(string mimeType)
        {
            // Act
            Action act = () => MimeTypeHelper.GetExtension(mimeType);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void GetExtension_throws_when_mimeType_starts_with_dot()
        {
            // Act
            Action act = () => MimeTypeHelper.GetExtension(".exe");

            // Assert
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void GetExtension_throws_when_mimeType_not_found()
        {
            // Act
            Action act = () => MimeTypeHelper.GetExtension("my-test-mime-type");

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void GetExtension_return_empty_string_when_mimeType_not_found_and_throwIfNotFound_false()
        {
            // Act
            var result = MimeTypeHelper.GetExtension("my-test-mime-type", false);

            // Assert
            result.Should().Be(string.Empty);
        }

        [Theory]
        [ClassData(typeof(GetExtensionsTheoryData))]
        public void GetExtensions(string mimeType, IEnumerable<string> expected)
        {
            // Act
            var result = MimeTypeHelper.GetExtensions(mimeType);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        private class GetExtensionsTheoryData : TheoryData<string, IEnumerable<string>>
        {
            public GetExtensionsTheoryData()
            {
                Add("text/scriptlet", new List<string> { ".sct", ".wsc" });
                Add("application/x-perfmon", new List<string> { ".pma", ".pmc", ".pml", ".pmr", ".pmw" });
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void GetExtensions_throws_when_mimeType_is_null_or_empty_with_whitespace(string mimeType)
        {
            // Act
            Action act = () => MimeTypeHelper.GetExtensions(mimeType);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void GetExtensions_throws_when_mimeType_starts_with_dot()
        {
            // Act
            Action act = () => MimeTypeHelper.GetExtensions(".exe");

            // Assert
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void GetExtensions_throws_when_mimeType_not_found()
        {
            // Act
            Action act = () => MimeTypeHelper.GetExtensions("my-test-mime-type");

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void GetExtensions_return_empty_list_when_mimeType_not_found_and_throwIfNotFound_false()
        {
            // Act
            var result = MimeTypeHelper.GetExtensions("my-test-mime-type", false);

            // Assert
            result.Should().BeEquivalentTo(new List<string>());
        }
    }
}
