using System;
using System.Collections.Generic;

namespace Structr.Operations.Internal
{
    internal static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            return (T)serviceProvider.GetService(typeof(T));
        }

        public static IEnumerable<T> GetServices<T>(this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            return serviceProvider.GetService<IEnumerable<T>>();
        }
    }
}
