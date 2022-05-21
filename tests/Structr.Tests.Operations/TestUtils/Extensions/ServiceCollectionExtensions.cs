using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Tests.Operations.TestUtils.Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSpyWriter(this IServiceCollection services, IStringWriter spyWriterToInject)
        {
            services.TryAdd(new ServiceDescriptor(typeof(IStringWriter), spyWriterToInject));
            return services;
        }
    }
}
