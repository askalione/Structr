using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Navigation
{
    public class NavigationBuilder : INavigationBuilder
    {
        private readonly NavigationConfigurator _configurator;

        public NavigationBuilder(NavigationConfigurator configurator)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            _configurator = configurator;
        }

        public IEnumerable<TNavigationItem> BuildNavigation<TNavigationItem>() where TNavigationItem : NavigationItem<TNavigationItem>
        {
            var configuration = _configurator.Get<TNavigationItem>();
            if (configuration == null)
            {
                throw new InvalidOperationException($"Navigation configuration of type {typeof(TNavigationItem).FullName} not found.");
            }


        }
    }
}
