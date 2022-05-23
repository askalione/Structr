using Structr.Navigation.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Structr.Navigation
{
    /// <inheritdoc cref="INavigationBuilder{TNavigationItem}"/>
    public class NavigationBuilder<TNavigationItem> : INavigationBuilder<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        private readonly INavigationProvider<TNavigationItem> _provider;
        private readonly NavigationOptions<TNavigationItem> _options;
        private readonly INavigationCache _cache;

        private Type _navigationItemType;
        private IEnumerable<PropertyInfo> _navigationItemTypeProperties;
        private bool _hasActiveNavigationItem;

        /// <summary>
        /// Initializes an instance of <see cref="NavigationBuilder{TNavigationItem}"/>.
        /// </summary>
        /// <param name="provider">The <see cref="INavigationProvider{TNavigationItem}"/>.</param>
        /// <param name="options">The <see cref="NavigationOptions{TNavigationItem}"/>.</param>
        /// <param name="cache">The <see cref="INavigationCache"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="provider"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="cache"/> is <see langword="null"/>.</exception>
        public NavigationBuilder(INavigationProvider<TNavigationItem> provider,
            NavigationOptions<TNavigationItem> options,
            INavigationCache cache)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            _provider = provider;
            _options = options;
            _cache = cache;
        }

        public IEnumerable<TNavigationItem> BuildNavigation()
        {
            _navigationItemType = typeof(TNavigationItem);
            _navigationItemTypeProperties = _navigationItemType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.CanWrite == true)
                .ToList();

            var navItems = _options.EnableCaching == true
                ? _cache.GetOrAdd(CreateNavigation)
                : CreateNavigation();

            navItems = BuildNavigation(navItems);

            return navItems;
        }

        private IEnumerable<TNavigationItem> BuildNavigation(IEnumerable<TNavigationItem> srcNavItems)
        {
            var navItems = new List<TNavigationItem>();
            foreach (var srcNavItem in srcNavItems)
            {
                var navItem = HandleNavigationItem(srcNavItem);
                if (navItem != null)
                {
                    HandleNavigationItemChildren(navItem, srcNavItem);
                    navItems.Add(navItem);
                }
            }
            return navItems;
        }

        private void HandleNavigationItemChildren(TNavigationItem navItem, TNavigationItem srcNavItem)
        {
            foreach (var srcNavItemChild in srcNavItem.Children)
            {
                var navItemChild = HandleNavigationItem(srcNavItemChild);
                if (navItemChild != null)
                {
                    navItem.AddChild(navItemChild);
                    HandleNavigationItemChildren(navItemChild, srcNavItemChild);
                }
            }
        }

        private TNavigationItem HandleNavigationItem(TNavigationItem srcNavItem)
        {
            TNavigationItem navItem = null;

            if (_options.ItemFilter?.Invoke(srcNavItem) == true)
            {
                navItem = new TNavigationItem();
                foreach (var prop in _navigationItemTypeProperties)
                {
                    prop.SetValue(navItem, prop.GetValue(srcNavItem, null), null);
                }
                if (_hasActiveNavigationItem == false)
                {
                    navItem.SetActive(_options.ItemActivator?.Invoke(srcNavItem) == true);
                    if (navItem.IsActive == true)
                    {
                        _hasActiveNavigationItem = true;
                    }
                }

                return navItem;
            }

            return navItem;
        }

        private IEnumerable<TNavigationItem> CreateNavigation()
        {
            var navItems = _provider.CreateNavigation();

            if (navItems != null)
            {
                var resourceType = _options.ResourceType;
                if (resourceType != null)
                {
                    var resourceManager = ResourceProvider.TryGetResourceManager(resourceType);
                    if (resourceManager != null)
                    {
                        LoadResource(navItems, resourceManager);
                        foreach (var navItem in navItems)
                        {
                            LoadResource(navItem.Descendants, resourceManager);
                        }
                    }
                }
            }

            return navItems;
        }

        private void LoadResource(IEnumerable<TNavigationItem> navItems, ResourceManager resourceManager)
        {
            foreach (var navItem in navItems)
            {
                var resourceName = string.IsNullOrWhiteSpace(navItem.ResourceName) == false
                    ? navItem.ResourceName
                    : navItem.Id;
                if (string.IsNullOrEmpty(resourceName) == false)
                {
                    var resourceString = resourceManager.GetString(resourceName);
                    if (resourceString != null)
                    {
                        navItem.Title = resourceString;
                    }
                }
            }
        }
    }
}
