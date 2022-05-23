using System.IO;
using System.Reflection;

namespace Structr.Tests.Navigation.TestUtils
{
    internal static class TestDataDirectoryPath
    {
        public static string Combine(string fileName)
        {
            var result = Path.Combine(
                new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.Parent.Parent.FullName,
                "TestData",
                fileName);
            return result;
        }
    }
}
