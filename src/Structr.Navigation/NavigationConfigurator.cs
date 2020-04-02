using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Structr.Navigation
{
    public class NavigationConfigurator
    {
        private readonly IServiceCollection _services;

        public NavigationConfigurator(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            _services = services;
        }

        public void Add<TNavigationItem>(INavigationProvider provider, NavigationItemOptions<TNavigationItem> options = null)
            where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (options == null)
                options = new NavigationItemOptions<TNavigationItem>();

            var configuration = new NavigationConfiguration<TNavigationItem>(provider, options);
            _services.TryAddSingleton(configuration);
        }
    }
}
