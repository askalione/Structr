using System.IO;
using System.Reflection;

namespace Structr.Samples
{
    public static class AppHelper
    {
        public static string RootPath
            => new DirectoryInfo(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .Parent
                .Parent
                .Parent
                .FullName;

        public static string ExecutablePath
            => Directory.GetCurrentDirectory();

        public static string GetRootPath(string path)
            => Path.Combine(RootPath, path);

        public static string GetExecutablePath(string path)
            => Path.Combine(ExecutablePath, path);
    }
}
