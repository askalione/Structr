using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Navigation
{
    public class Navigation<TMenuItem> : INavigation<TMenuItem> where TMenuItem : NavigationItem<TMenuItem>
    {
        private readonly IEnumerable<TMenuItem> _items;

        public TMenuItem Active => _items.FirstOrDefault(x => x.IsActive)
            ?? _items
                .SelectMany(x => x.Descendants)
                .FirstOrDefault(x => x.IsActive);

        public Navigation(INavigationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            _items = builder.Build<TMenuItem>();
        }

        public IEnumerator<TMenuItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
