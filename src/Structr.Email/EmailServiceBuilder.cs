using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Email
{
    public class EmailServiceBuilder
    {
        public IServiceCollection Services { get; }

        internal EmailServiceBuilder(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Services = services;
        }
    }
}
