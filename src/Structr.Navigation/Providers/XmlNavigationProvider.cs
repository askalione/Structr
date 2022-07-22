using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Structr.Navigation.Providers
{
    /// <summary>
    /// Provides functionality for creation list of navigation items <see cref="IEnumerable{TNavigationItem}"/> from XML file.
    /// </summary>
    /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
    public class XmlNavigationProvider<TNavigationItem> : INavigationProvider<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        private readonly string _path;

        /// <summary>
        /// Initializes an instance of <see cref="JsonNavigationProvider{TNavigationItem}"/>.
        /// </summary>
        /// <param name="path">Absolute path to XML file with navigation configuration.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/>.</exception>
        public XmlNavigationProvider(string path)
        {
            if (string.IsNullOrWhiteSpace(path) == true)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _path = path;
        }

        public IEnumerable<TNavigationItem> CreateNavigation()
        {
            if (File.Exists(_path) == false)
            {
                throw new FileNotFoundException("Navigation file not found.", _path);
            }

            var xml = XDocument.Load(_path);
            if (xml == null)
            {
                throw new InvalidOperationException("Invalid navigation xml file.");
            }

            xml.Declaration = null;
            var navItems = new List<TNavigationItem>();
            var xmlElements = xml.Root.Elements();

            foreach (var xmlElement in xmlElements)
            {
                var navItem = ParseNavigationItem(xmlElement);
                navItems.Add(navItem);
            }

            return navItems;
        }

        private TNavigationItem ParseNavigationItem(XElement xmlElement)
        {
            var navItem = new TNavigationItem();
            var navItemType = navItem.GetType();
            var navItemTypeProps = navItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var xmlAttributes = xmlElement.Attributes();
            foreach (var xmlAttribute in xmlAttributes)
            {
                if (xmlAttribute.Value != null)
                {
                    var prop = navItemTypeProps
                        .FirstOrDefault(x => x.Name.Equals(xmlAttribute.Name.ToString(), StringComparison.OrdinalIgnoreCase) == true);
                    if (prop != null)
                    {
                        if (prop.CanWrite == true)
                        {
                            prop.SetValue(navItem, Convert.ChangeType(xmlAttribute.Value, prop.PropertyType), null);
                        }
                    }
                }
            }

            var xmlElementChildren = xmlElement.Elements();
            if (xmlElementChildren != null && xmlElementChildren.Any() == true)
            {
                foreach (var xmlElementChild in xmlElementChildren)
                {
                    var navItemChild = ParseNavigationItem(xmlElementChild);
                    if (navItemChild != null)
                    {
                        navItem.AddChild(navItemChild);
                    }
                }
            }

            return navItem;
        }
    }
}
