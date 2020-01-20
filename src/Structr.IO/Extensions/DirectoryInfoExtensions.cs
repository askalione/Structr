using System;
using System.IO;

namespace Structr.IO.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo GetParent(this DirectoryInfo currentDir, int level)
        {
            if (currentDir == null)
                throw new ArgumentNullException(nameof(currentDir));
            if (level <= 0)
                throw new ArgumentOutOfRangeException(nameof(level), level, "Parent level must greater then 0");

            var parentDir = currentDir;

            for (var i = 1; i <= level; i++)
                parentDir = parentDir?.Parent;

            return parentDir;
        }
    }
}
