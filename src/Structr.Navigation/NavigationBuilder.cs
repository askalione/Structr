using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Structr.Navigation.Internal;
using System;
using System.Collections.Generic;
using System.Resources;

namespace Structr.Navigation
{
    public class NavigationBuilder : INavigationBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;

        public NavigationBuilder(IServiceProvider serviceProvider, IMemoryCache memoryCache)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (memoryCache == null)
                throw new ArgumentNullException(nameof(memoryCache));

            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
        }

        public IEnumerable<TNavigationItem> Build<TNavigationItem>() where TNavigationItem : NavigationItem<TNavigationItem>
        {
            var configuration = (NavigationConfiguration<TNavigationItem>)_serviceProvider.GetService(typeof(NavigationConfiguration<TNavigationItem>));
            if (configuration == null)
                throw new InvalidOperationException($"Navigation configuration of type {typeof(TNavigationItem).FullName} not found.");

            var provider = configuration.Provider;
            var options = configuration.Options;

            var nav = new List<TNavigationItem>();
            var navItemsIds = new List<string>();
            var jNav = GetNavigation<TNavigationItem>(provider, options.Cache);

            if (jNav != null && jNav.HasValues)
            {
                var resourceManager = ResourceProvider.TryGetResourceManager(options.Resource);
                var navItemFilter = options.Filter ?? DefaultFilter<TNavigationItem>();
                var navItemActivator = options.Activator ?? DefaultActivator<TNavigationItem>();
                foreach (var jNavItem in jNav.Children<JObject>())
                {
                    var navItem = TryBuildNavigationItem(jNavItem, navItemFilter, navItemActivator, resourceManager, nav, navItemsIds);
                    if (navItem != null)
                        nav.Add(navItem);
                }
            }

            return nav;
        }

        private JArray GetNavigation<TNavigationItem>(INavigationProvider provider, bool cache) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            JArray factory() => provider.GetNavigation();
            JArray jNav = cache && _memoryCache != null
                ? NavigationCache.GetOrAdd<TNavigationItem>(_memoryCache, factory)
                : factory();
            return jNav;
        }

        private TNavigationItem TryBuildNavigationItem<TNavigationItem>(JObject jNavItem,
            Func<TNavigationItem, IServiceProvider, bool> navItemFilter,
            Func<TNavigationItem, IServiceProvider, bool> navItemActivator,
            ResourceManager resourceManager,
            IEnumerable<TNavigationItem> nav,
            List<string> navItemsIds) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            var navItem = jNavItem.ToObject<TNavigationItem>(new JsonSerializer()
            {
                ContractResolver = new PrivateSetterContractResolver()
            });
            if (navItem == null)
                return null;

            ValidateOrThrowNavItemId(navItem.Id, navItemsIds);
            if (navItemFilter(navItem, _serviceProvider))
            {
                navItem.IsActive = navItemActivator(navItem, _serviceProvider);
                if (navItem.IsActive)
                    DeactivateAllExceptOne(nav, navItem);
                TryResource(navItem, resourceManager);
                Build(jNavItem, navItem, navItemFilter, navItemActivator, resourceManager, nav, navItemsIds);
                return navItem;
            }
            else
            {
                return null;
            }
        }

        private static void TryResource<TNavigationItem>(TNavigationItem navItem, ResourceManager resourceManager) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (resourceManager == null)
                return;

            string resourceString = resourceManager.GetString(navItem.Id);
            if (resourceString != null)
                navItem.Title = resourceString;
        }

        private void Build<TNavigationItem>(JObject jParentNavItem,
            TNavigationItem ParentNavItem,
            Func<TNavigationItem, IServiceProvider, bool> navItemFilter,
            Func<TNavigationItem, IServiceProvider, bool> navItemActivator,
            ResourceManager resourceManager,
            IEnumerable<TNavigationItem> nav,
            List<string> navItemsIds) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            var jChildren = jParentNavItem[nameof(InternalNavigationItem.Children)] as JArray;
            if (jChildren != null)
            {
                foreach (var jNavItem in jChildren.Children<JObject>())
                {
                    var navItem = TryBuildNavigationItem(jNavItem, navItemFilter, navItemActivator, resourceManager, nav, navItemsIds);
                    if (navItem != null)
                    {
                        ParentNavItem.AddChild(navItem);
                    }
                }
            }
        }

        private static Func<TNavigationItem, IServiceProvider, bool> DefaultFilter<TNavigationItem>() where TNavigationItem : NavigationItem<TNavigationItem>
            => (item, serviceProvider) => true;

        private static Func<TNavigationItem, IServiceProvider, bool> DefaultActivator<TNavigationItem>() where TNavigationItem : NavigationItem<TNavigationItem>
            => (item, serviceProvider) => false;

        private static void DeactivateAllExceptOne<TNavigationItem>(IEnumerable<TNavigationItem> navigation, TNavigationItem except)
            where TNavigationItem : NavigationItem<TNavigationItem>
        {
            foreach (var item in navigation)
            {
                if (item != except)
                    item.IsActive = false;
                foreach(var descendant in item.Descendants)
                {
                    if (descendant != except)
                        descendant.IsActive = false;
                }
            }
        }

        private static void ValidateOrThrowNavItemId(string id, List<string> ids)
        {
            if (string.IsNullOrEmpty(id))
                throw new InvalidOperationException("Navigation item id must be not null or empty.");

            var theId = id.Trim().ToUpper();
            if (ids.Contains(theId))
                throw new InvalidOperationException($"Navigation item id `{id}` must be unique.");

            ids.Add(theId);
        }
    }
}
