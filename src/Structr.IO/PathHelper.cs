using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Structr.IO
{
    public static class PathHelper
    {
        private static Lazy<PathOptions> _optionsProvider = new Lazy<PathOptions>(() => new PathOptions());
        public static PathOptions Options => _optionsProvider.Value;

        public static void Configure(Action<PathOptions> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            action(Options);
        }

        public static string Combine(Directory directory, string path)
        {
            return Path.Combine(GetPath(directory), path);
        }

        public static string Format(string path)
        {
            return Format(path, Enum.GetValues(typeof(Directory)).Cast<Directory>().ToArray());
        }

        public static string Format(string path, Directory[] directories)
        {
            if (directories == null)
                throw new ArgumentNullException(nameof(directories));

            string formattedPath = path;

            foreach (Directory directory in directories)
                formattedPath = Format(formattedPath, directory);

            return formattedPath;
        }

        public static string Format(string path, Directory directory)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            string formattedPath = path.Replace('/', '\\');

            if (formattedPath.IndexOf(Options.Template(directory), StringComparison.OrdinalIgnoreCase) >= 0)
                formattedPath = formattedPath.Replace(Options.Template(directory), GetPath(directory), StringComparison.OrdinalIgnoreCase);

            return Regex.Replace(formattedPath, "\\\\{2,}", @"\");
        }

        private static string GetPath(Directory directory)
        {
            Options.Directories.TryGetValue(directory, out string path);
            return path?.Trim() ?? "";
        }

        private static string Replace(this string @string, string oldValue, string newValue, StringComparison comparisonType)
        {
            if (@string == null)
                throw new ArgumentNullException(nameof(@string));
            if (@string.Length == 0)
                return @string;
            if (oldValue == null)
                throw new ArgumentNullException(nameof(oldValue));
            if (oldValue.Length == 0)
                throw new ArgumentException("String cannot be of zero length.");

            StringBuilder resultStringBuilder = new StringBuilder(@string.Length);

            bool isReplacementNullOrEmpty = string.IsNullOrEmpty(@newValue);

            const int valueNotFound = -1;
            int foundAt;
            int startSearchFromIndex = 0;
            while ((foundAt = @string.IndexOf(oldValue, startSearchFromIndex, comparisonType)) != valueNotFound)
            {

                int @charsUntilReplacment = foundAt - startSearchFromIndex;
                bool isNothingToAppend = @charsUntilReplacment == 0;
                if (!isNothingToAppend)
                {
                    resultStringBuilder.Append(@string, startSearchFromIndex, @charsUntilReplacment);
                }

                if (!isReplacementNullOrEmpty)
                {
                    resultStringBuilder.Append(@newValue);
                }

                startSearchFromIndex = foundAt + oldValue.Length;
                if (startSearchFromIndex == @string.Length)
                {
                    return resultStringBuilder.ToString();
                }
            }

            int @charsUntilStringEnd = @string.Length - startSearchFromIndex;
            resultStringBuilder.Append(@string, startSearchFromIndex, @charsUntilStringEnd);

            return resultStringBuilder.ToString();
        }
    }
}
