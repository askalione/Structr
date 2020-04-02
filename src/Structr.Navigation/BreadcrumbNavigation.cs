using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Navigation
{
    public class BreadcrumbNavigation<TBreadcrumb> : IBreadcrumbNavigation<TBreadcrumb> where TBreadcrumb : NavigationItem<TBreadcrumb>
    {
        private readonly IEnumerable<TBreadcrumb> _items;

        public TBreadcrumb Active => _items.FirstOrDefault(x => x.IsActive);

        public BreadcrumbNavigation(INavigationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var navigation = builder.Build<TBreadcrumb>();
            var activeNavItem = navigation.FirstOrDefault(x => x.IsActive)
                ?? navigation
                    .SelectMany(x => x.Descendants)
                    .FirstOrDefault(x => x.IsActive);
            var breadcrumbs = activeNavItem.Ancestors.Reverse().ToList();
            breadcrumbs.Add(activeNavItem);

            _items = breadcrumbs;
        }

        public IEnumerator<TBreadcrumb> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
