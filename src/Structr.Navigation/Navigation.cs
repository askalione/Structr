using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Navigation
{
    /// <inheritdoc cref="INavigation{TNavigationItem}"/>
    public class Navigation<TNavigationItem> : INavigation<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        private readonly IEnumerable<TNavigationItem> _items;

        public TNavigationItem Active { get; }

        /// <summary>
        /// Initializes an instance of <see cref="Navigation{TNavigationItem}"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INavigationBuilder{TNavigationItem}"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="builder"/> is <see langword="null"/>.</exception>
        public Navigation(INavigationBuilder<TNavigationItem> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            _items = builder.BuildNavigation();
            Active = _items.FirstOrDefault(x => x.IsActive)
                ?? _items.SelectMany(x => x.Descendants).FirstOrDefault(x => x.IsActive);
        }

        public IEnumerator<TNavigationItem> GetEnumerator()
            => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
