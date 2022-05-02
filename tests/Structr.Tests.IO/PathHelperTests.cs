using Structr.IO;
using System.IO;
using Xunit;

namespace Structr.Tests.IO
{
    public class PathHelperTests
    {
        [Fact]
        public void Combine_DataDirectoryAndRelativePath_CombinedFullPath()
        {
            var relativePath = @"foo\bar\baz.txt";

            PathHelper.Configure(options =>
            {
                options.Directories[ContentDirectory.Data] = PathDefaults.DataPath;
            });

            var expectedPath = Path.Combine(PathDefaults.DataPath, relativePath);
            var actualPath = PathHelper.Combine(ContentDirectory.Data, relativePath);

            Assert.Equal(expectedPath, actualPath);
        }

        [Fact]
        public void Format_TemplatedPath_FormattedPath()
        {
            var path = @"|DataDirectory|\foo\bar\baz.txt";

            PathHelper.Configure(options =>
            {
                options.Template = (directory) => $"|{directory}Directory|";
                options.Directories[ContentDirectory.Data] = PathDefaults.DataPath;
            });

            var expectedPath = path.Replace(PathHelper.Options.Template(ContentDirectory.Data), PathDefaults.DataPath);
            var actualPath = PathHelper.Format(path);

            Assert.Equal(expectedPath, actualPath);
        }
    }
}
