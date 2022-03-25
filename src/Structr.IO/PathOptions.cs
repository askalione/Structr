using System;
using System.Collections.Generic;

namespace Structr.IO
{
    public class PathOptions
    {
        public Func<ContentDirectory, string> Template { get; set; }
        public Dictionary<ContentDirectory, string> Directories { get; }

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
