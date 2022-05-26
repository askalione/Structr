using Structr.IO;

namespace Structr.Tests.IO.TestUtils
{
    internal class PathHelperFixture
    {
        public PathHelperFixture()
        {
            PathHelper.Configure(options =>
            {
                options.Template = (directory) => $"|{directory}Directory|";
                options.Directories[ContentDirectory.Base] = ContentDirectoryDefaults.Base;
                options.Directories[ContentDirectory.Data] = ContentDirectoryDefaults.Data;
            });
        }
    }
}
