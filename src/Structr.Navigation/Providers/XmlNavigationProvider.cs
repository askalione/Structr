using Newtonsoft.Json.Linq;
using Structr.Navigation.Internal;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Structr.Navigation.Providers
{
    public class XmlNavigationProvider : INavigationProvider
    {
        private readonly string _path;

        public XmlNavigationProvider(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));            

            _path = path;
        }

        public JArray GetNavigation()
        {
            if (!File.Exists(_path))
                throw new FileNotFoundException($"Navigation file not found.", _path);

            var xml = XDocument.Load(_path);
            if (xml == null)
                throw new InvalidOperationException("Invalid navigation xml file.");

            xml.Declaration = null;

            var jNav = new JArray();
            foreach (var xNode in xml.Root.Elements())
            {
                var jNavItem = new JObject();
                Parse(xNode, jNavItem);
                jNav.Add(jNavItem);
            }

            return jNav;
        }

        private static void Parse(XElement xNode, JObject jNavItem)
        {
            var xAttributes = xNode.Attributes();
            foreach (var xAttribute in xAttributes)
            {
                jNavItem.Add(new JProperty(xAttribute.Name.ToString(), xAttribute.Value));
            }
            var xChildElements = xNode.Elements();
            if (xChildElements.Any())
            {
                var jChildren = new JArray();
                foreach (var xChildNode in xChildElements)
                {
                    var jChildNavItem = new JObject();
                    Parse(xChildNode, jChildNavItem);
                    jChildren.Add(jChildNavItem);
                }
                jNavItem.Add(new JProperty(nameof(InternalNavigationItem.Children), jChildren));
            }
        }
    }
}
