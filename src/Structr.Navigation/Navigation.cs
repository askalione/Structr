using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Navigation
{
    public class Navigation<TNavigationItem> : INavigation<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        private readonly IEnumerable<TNavigationItem> _items;

        public TNavigationItem Active { get; private set; }

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
