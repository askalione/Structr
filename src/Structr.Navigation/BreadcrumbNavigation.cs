using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Navigation
{
    /// <inheritdoc cref="IBreadcrumbNavigation{TNavigationItem}"/>
    public class BreadcrumbNavigation<TNavigationItem> : IBreadcrumbNavigation<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        private readonly IEnumerable<TNavigationItem> _items;

        public TNavigationItem Active { get; }

        /// <summary>
        /// Initializes an instance of <see cref="BreadcrumbNavigation{TNavigationItem}"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INavigationBuilder{TNavigationItem}"/>.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="builder"/> is <see langword="null"/>.</exception>
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
