using System;
using System.Collections.Generic;

namespace Structr.IO
{
    /// <summary>
    /// Defines a set of options used in <see cref="PathHelper"/>.
    /// </summary>
    public class PathOptions
    {
        /// <summary>
        /// Determines templates for content directories.
        /// </summary>
        public Func<ContentDirectory, string> Template { get; set; }

        /// <summary>
        /// Determines absolute paths for content directories.
        /// </summary>
        public Dictionary<ContentDirectory, string> Directories { get; }

        /// <summary>
        /// Initializes an instance of <see cref="PathOptions"/> with default values.
        /// </summary>
        public PathOptions()
        {
            Template = (directory) => $"|{directory}Directory|";
            Directories = new Dictionary<ContentDirectory, string>
            {
                { ContentDirectory.Base, AppDomain.CurrentDomain?.BaseDirectory ?? "" },
                { ContentDirectory.Data, AppDomain.CurrentDomain?.GetData("DataDirectory")?.ToString() ?? "" }
            };
        }
    }
}
