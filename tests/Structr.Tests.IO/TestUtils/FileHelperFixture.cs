using System;
using System.IO;

namespace Structr.Tests.IO.TestUtils
{
    public class FileHelperFixture : IDisposable
    {
        public static void DeleteTestFiles()
        {
            if (File.Exists(TestDataPath.Path))
            {
                File.Delete(TestDataPath.Path);
            }
            if (File.Exists(TestDataPath.NextUniquePath))
            {
                File.Delete(TestDataPath.NextUniquePath);
            }
            var nonExistentDirectory = Path.GetDirectoryName(TestDataPath.NonExistentPath);
            if (Directory.Exists(nonExistentDirectory))
            {
                Directory.Delete(nonExistentDirectory, true);
            }
        }

        public void Dispose()
        {
            DeleteTestFiles();
        }
    }
}
