using System;
using System.IO;

namespace Structr.Tests.IO.TestUtils
{
    public class FileHelperFixture : IDisposable
    {
        public string Text { get; private set; } = "Hello world!";
        public string Path { get; private set; }
        public string NextUniquePath { get; private set; }
        public string NonExistentPath { get; private set; }

        public FileHelperFixture()
        {
            var dir = TestDataPath.Combine(ContentDirectoryDefaults.Data);
            Path = $"{dir}\\readme.txt";
            NextUniquePath = $"{dir}\\readme_1.txt";
            NonExistentPath = $"{dir}\\NotExistDirectory\\readme.txt";

            if (File.Exists(Path) == false)
            {
                File.WriteAllText(Path, Text);
            }
        }

        public void Dispose()
        {
            if (File.Exists(Path) == false)
            {
                File.WriteAllText(Path, Text);
            }
        }
    }
}
