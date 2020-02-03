using System;
using System.IO;

namespace Structr.Abstractions.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo GetParent(this DirectoryInfo source, int level)
        {
            Ensure.NotNull(source, nameof(source));

            if (level < 1)
                throw new ArgumentOutOfRangeException(nameof(level), level, "Level must be greater or equal 1");

            var parent = source;

            for (var i = 1; i <= level; i++)
                parent = parent?.Parent;

            return parent;
        }
    }
}
