using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Structr.IO
{
    /// <summary>
    /// Provides methods for combine and format <see cref="ContentDirectory"/> paths.
    /// </summary>
    public static class PathHelper
    {
        private static Lazy<PathOptions> _optionsProvider = new Lazy<PathOptions>(() => new PathOptions());

        /// <summary>
        /// The <see cref="PathOptions"/>.
        /// </summary>
        public static PathOptions Options => _optionsProvider.Value;

        /// <summary>
        /// Configure <see cref="PathOptions"/>.
        /// </summary>
        /// <param name="action">The delegate to configure <see cref="PathOptions"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="action"/> is <see langword="null"/>.</exception>
        public static void Configure(Action<PathOptions> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action(Options);
        }

        /// <summary>
        /// Combines <see cref="ContentDirectory"/> absolute path and relative <paramref name="path"/> into a common path.
        /// </summary>
        /// <param name="directory">The <see cref="ContentDirectory"/>.</param>
        /// <param name="path">Relative path.</param>
        /// <returns>Common path = <see cref="ContentDirectory"/> path + <paramref name="path"/>.</returns>
        public static string Combine(ContentDirectory directory, string path)
            => Path.Combine(GetPath(directory), path);

        /// <inheritdoc cref="Format(string, ContentDirectory[])"/>
        public static string Format(string path)
            => Format(path, Enum.GetValues(typeof(ContentDirectory)).Cast<ContentDirectory>().ToArray());

        /// <summary>
        /// Replace content directory templates from path to content directory absolute paths.
        /// </summary>
        /// <param name="path">The path with content directory templates.</param>
        /// <param name="directories">The list of <see cref="ContentDirectory"/>.</param>
        /// <returns>The absolute path to content directory.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public static string Format(string path, ContentDirectory[] directories)
        {
            if (directories == null)
            {
                throw new ArgumentNullException(nameof(directories));
            }

            string formattedPath = path;
            foreach (ContentDirectory directory in directories)
            {
                formattedPath = Format(formattedPath, directory);
            }
            return formattedPath;
        }

        /// <summary>
        /// Replace a content directory template from path to a content directory absolute path.
        /// </summary>
        /// <param name="path">The path with the content directory template.</param>
        /// <param name="directory">The <see cref="ContentDirectory"/>.</param>
        /// <returns>The absolute path to content directory.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public static string Format(string path, ContentDirectory directory)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            string formattedPath = path.Replace('/', '\\');

            if (formattedPath.IndexOf(Options.Template(directory), StringComparison.OrdinalIgnoreCase) >= 0)
            {
                formattedPath = formattedPath.Replace(Options.Template(directory), GetPath(directory), StringComparison.OrdinalIgnoreCase);
            }

            string result = Regex.Replace(formattedPath, "\\\\{2,}", @"\");
            return result;
        }

        private static string GetPath(ContentDirectory directory)
        {
            Options.Directories.TryGetValue(directory, out string path);
            string result = path?.Trim() ?? "";
            return result;
        }

        private static string Replace(this string @string, string oldValue, string newValue, StringComparison comparisonType)
        {
            if (@string == null)
            {
                throw new ArgumentNullException(nameof(@string));
            }
            if (@string.Length == 0)
            {
                return @string;
            }
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }
            if (oldValue.Length == 0)
            {
                throw new ArgumentException("String cannot be of zero length.");
            }

            StringBuilder resultStringBuilder = new StringBuilder(@string.Length);

            bool isReplacementNullOrEmpty = string.IsNullOrEmpty(@newValue);

            const int valueNotFound = -1;
            int foundAt;
            int startSearchFromIndex = 0;
            while ((foundAt = @string.IndexOf(oldValue, startSearchFromIndex, comparisonType)) != valueNotFound)
            {
                int @charsUntilReplacment = foundAt - startSearchFromIndex;
                bool isNothingToAppend = @charsUntilReplacment == 0;
                if (isNothingToAppend == false)
                {
                    resultStringBuilder.Append(@string, startSearchFromIndex, @charsUntilReplacment);
                }

                if (isReplacementNullOrEmpty == false)
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
