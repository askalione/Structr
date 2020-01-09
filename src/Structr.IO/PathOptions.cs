using System;
using System.Collections.Generic;

namespace Structr.IO
{
    public class PathOptions
    {
        public Func<EDirectory, string> Template { get; set; }
        public Dictionary<EDirectory, string> Directories { get; }

        public PathOptions()
        {
            Template = (directory) => $"|{directory}Directory|";
            Directories = new Dictionary<EDirectory, string>
            {
                { EDirectory.Base, AppDomain.CurrentDomain?.BaseDirectory ?? "" },
                { EDirectory.Data, AppDomain.CurrentDomain?.GetData(EDirectory.Data.ToString())?.ToString() ?? "" }
            };
        }
    }
}
