using System;
using System.IO;

namespace Structr.Tests.Abstractions.TestsUtils
{
    public class FileSystemTestFixture : IDisposable
    {
        public const string _testingPath = "tests_temp";

        public FileSystemTestFixture()
        {
            Directory.CreateDirectory(_testingPath);
        }

        public static string GetTestingPath(Type testClass)
        {
            return Path.GetFullPath(Path.Combine(_testingPath, testClass.Name));
        }

        public void Dispose()
        {
            Directory.Delete(_testingPath, true);
        }
    }
}
