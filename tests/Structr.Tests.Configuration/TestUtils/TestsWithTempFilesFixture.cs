using System;
using System.IO;

namespace Structr.Tests.Configuration.TestUtils
{
    public class TestsWithTempFilesFixture : IDisposable
    {
        public TestsWithTempFilesFixture()
        {
            var tempDirectoryPath = TestDataPath.CombineWithTemp("");
            if (Directory.Exists(tempDirectoryPath) == true)
            {
                Directory.Delete(tempDirectoryPath, true);
            }
            Directory.CreateDirectory(tempDirectoryPath);
        }

        public void Dispose()
        {
            var tempDirectoryPath = TestDataPath.CombineWithTemp("");
            if (Directory.Exists(tempDirectoryPath) == true)
            {
                Directory.Delete(tempDirectoryPath, true);
            }
        }
    }
}
