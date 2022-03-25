using System;
using System.Collections.Generic;

namespace Structr.IO
{
    public class PathOptions
    {
        public Func<Directory, string> Template { get; set; }
        public Dictionary<Directory, string> Directories { get; }

        public PathOptions()
        {
            Template = (directory) => $"|{directory}Directory|";
            Directories = new Dictionary<Directory, string>
            {
                { Directory.Base, AppDomain.CurrentDomain?.BaseDirectory ?? "" },
                { Directory.Data, AppDomain.CurrentDomain?.GetData("DataDirectory")?.ToString() ?? "" }
            };
        }
    }
}
