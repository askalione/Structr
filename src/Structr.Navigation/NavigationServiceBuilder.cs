using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Navigation
{
    public class NavigationServiceBuilder
    {
        public IServiceCollection Services { get; }

        public NavigationServiceBuilder(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Services = services;
        }
    }
}
