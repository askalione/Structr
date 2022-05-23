using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace Structr.Tests.Operations.TestUtils.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMockWriter(this IServiceCollection services, IStringWriter spyWriterToInject)
        {
            services.TryAdd(new ServiceDescriptor(typeof(IStringWriter), spyWriterToInject));
            return services;
        }
    }
}
