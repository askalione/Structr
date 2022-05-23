using System;
using System.IO;

namespace Structr.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DirectoryInfo"/>.
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Gets DirectoryInfo for parent with specified level starting from nearest one with level 1
        /// </summary>
        /// <param name="source"></param>
        /// <param name="level">Parent level starting from nearest parent with number 1</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when level of parent lesser than 1</exception>
        public static DirectoryInfo GetParent(this DirectoryInfo source, int level)
        {
            Ensure.NotNull(source, nameof(source));

            if (level < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(level), level, "Level must be greater or equal 1");
            }

            var parent = source;

            for (var i = 1; i <= level; i++)
            {
                parent = parent?.Parent;
            }

            return parent;
        }
    }
}
