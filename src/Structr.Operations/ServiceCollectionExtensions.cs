using Microsoft.Extensions.DependencyInjection.Extensions;
using Scrutor;
using Structr.Operations;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOperations(this IServiceCollection services, params Assembly[] assembliesToScan)
            => AddOperations(services, null, assembliesToScan);

        public static IServiceCollection AddOperations(this IServiceCollection services,
            Action<OperationServiceOptions> configureOptions,
            params Assembly[] assembliesToScan)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (assembliesToScan == null)
                throw new ArgumentNullException(nameof(assembliesToScan));
            if (!assembliesToScan.Any())
                throw new ArgumentException("No assemblies found to scan. At least one assembly to scan for handlers is required");

            var options = new OperationServiceOptions();

            configureOptions?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(typeof(IOperationExecutor), options.ExecutorType, options.Lifetime));

            services.Scan(scan =>
               scan.FromAssemblies(assembliesToScan)
                   .AddClasses(classes => classes.AssignableTo(typeof(IOperationHandler<>)))
                   .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                   .AsImplementedInterfaces()
                   //.As(t => t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IOperationHandler<>)))
                   .WithTransientLifetime()
                   .AddClasses(classes => classes.AssignableTo(typeof(IOperationHandler<,>)))
                   .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                   .AsImplementedInterfaces()
                   //.As(t => t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IOperationHandler<,>)))
                   .WithTransientLifetime());

            return services;
        }
    }
}
