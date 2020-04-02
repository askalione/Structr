using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Structr.Navigation.Providers
{
    public class JsonNavigationProvider : INavigationProvider
    {
        private readonly string _path;

        public JsonNavigationProvider(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            
            _path = path;
        }

        public JArray GetNavigation()
        {
            if (!File.Exists(_path))
                throw new FileNotFoundException($"Navigation file not found.", _path);

            var jNav = JArray.Parse(File.ReadAllText(_path));
            return jNav;
        }
    }
}
