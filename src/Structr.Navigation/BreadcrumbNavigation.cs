using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Navigation
{
    public class BreadcrumbNavigation<TNavigationItem> : IBreadcrumbNavigation<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        private readonly IEnumerable<TNavigationItem> _items;

        public TNavigationItem Active { get; }

        public BreadcrumbNavigation(INavigationBuilder<TNavigationItem> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var navItems = builder.BuildNavigation();
            var breadcrumbs = new List<TNavigationItem>();

            Active = navItems.FirstOrDefault(x => x.IsActive)
                ?? navItems.SelectMany(x => x.Descendants).FirstOrDefault(x => x.IsActive);

            if (Active != null)
            {
                breadcrumbs = Active.Ancestors.ToList();
                breadcrumbs.Add(Active);
            }

            _items = breadcrumbs;
        }

        public IEnumerator<TNavigationItem> GetEnumerator()
            => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
