using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Email
{
    /// <summary>
    /// An instance for configuring Email services.
    /// </summary>
    public class EmailServiceBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where Email services are configured.
        /// </summary>
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
