using System.IO;

namespace Structr.Tests.IO.TestUtils
{
    internal static class TestDataPath
    {
        private static readonly string _dir = Combine(ContentDirectoryDefaults.Data);

        public static readonly string Path = $"{_dir}\\readme.txt";
        public static readonly string NextUniquePath = $"{_dir}\\readme_1.txt";
        public static readonly string NonExistentPath = $"{_dir}\\NotExistDirectory\\readme.txt";

        public static string ContentRootPath => System.IO.Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                "TestData");

        public static string Combine(string fileName)
            => System.IO.Path.Combine(ContentRootPath, fileName);


    }
}