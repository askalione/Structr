#nullable disable
using System.IO;

namespace Structr.Tests.SqlServer
{
    internal static class TestDataPath
    {
        static TestDataPath()
        {
            if (Directory.Exists(ContentRootPath) == false)
            {
                Directory.CreateDirectory(ContentRootPath);
            }
        }

        public static string ContentRootPath => Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            "TestData");

        public static string Combine(string fileName)
            => Path.Combine(ContentRootPath, fileName);
    }
}
