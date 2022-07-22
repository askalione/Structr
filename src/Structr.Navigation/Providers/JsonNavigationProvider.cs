using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Structr.Navigation.Internal;
using System;
using System.Collections.Generic;
using System.IO;

namespace Structr.Navigation.Providers
{
    /// <summary>
    /// Provides functionality for creation list of navigation items <see cref="IEnumerable{TNavigationItem}"/> from JSON file.
    /// </summary>
    /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
    public class JsonNavigationProvider<TNavigationItem> : INavigationProvider<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        private readonly string _path;
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer
        {
            ContractResolver = new PrivateSetterContractResolver()
        };

        /// <summary>
        /// Initializes an instance of <see cref="JsonNavigationProvider{TNavigationItem}"/>.
        /// </summary>
        /// <param name="path">Absolute path to JSON file with navigation configuration.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/>.</exception>
        public JsonNavigationProvider(string path)
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

            var navItems = new List<TNavigationItem>();
            var content = File.ReadAllText(_path);

            if (string.IsNullOrWhiteSpace(content) == false)
            {
                var navJson = JArray.Parse(content);
                if (navJson != null && navJson.HasValues == true)
                {
                    var navItemsJson = navJson.Children<JObject>();
                    foreach (var navItemJson in navItemsJson)
                    {
                        var navItem = ParseNavigationItem(navItemJson);
                        if (navItem != null)
                        {
                            navItems.Add(navItem);
                        }
                    }
                }
            }

            return navItems;
        }

        private TNavigationItem ParseNavigationItem(JObject navItemJson)
        {
            var navItem = navItemJson.ToObject<TNavigationItem>(_jsonSerializer);
            if (navItem == null)
            {
                return null;
            }

            var childrenJsonPropertyName = nameof(InternalNavigationItem.Children);
            if (navItemJson.ContainsKey(childrenJsonPropertyName))
            {
                var navItemChildrenJson = navItemJson[childrenJsonPropertyName] as JArray;
                if (navItemChildrenJson != null && navItemChildrenJson.HasValues == true)
                {
                    var navItemsJson = navItemChildrenJson.Children<JObject>();
                    foreach (var navItemChildJson in navItemsJson)
                    {
                        var navItemChild = ParseNavigationItem(navItemChildJson);
                        if (navItemChild != null)
                        {
                            navItem.AddChild(navItemChild);
                        }
                    }
                }
            }

            return navItem;
        }
    }
}
