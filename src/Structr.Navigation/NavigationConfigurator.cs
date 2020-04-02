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

        public void Add<TNavigationItem>(INavigationProvider provider, Action<NavigationItemOptions<TNavigationItem>> configure = null)
            where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

           var options = new NavigationItemOptions<TNavigationItem>();
            configure?.Invoke(options);

            var configuration = new NavigationConfiguration<TNavigationItem>(provider, options);
            _services.TryAddSingleton(configuration);
        }
    }
}
