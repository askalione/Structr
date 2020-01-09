using Microsoft.Extensions.DependencyInjection.Extensions;
using Scrutor;
using Structr.Stateflows;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateflows(this IServiceCollection services, params Assembly[] assembliesToScan)
            => AddStateflows(services, null, assembliesToScan);

        public static IServiceCollection AddStateflows(this IServiceCollection services,
            Action<StateflowServiceOptions> configureOptions,
            params Assembly[] assembliesToScan)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (assembliesToScan == null)
                throw new ArgumentNullException(nameof(assembliesToScan));
            if (!assembliesToScan.Any())
                throw new ArgumentException("No assemblies found to scan. At least one assembly to scan for state configurators and providers is required");

            var options = new StateflowServiceOptions();

            configureOptions?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(typeof(IStateMachineProvider), options.ProviderType, options.Lifetime));

            services.Scan(scan =>
               scan.FromAssemblies(assembliesToScan)
                   .AddClasses(classes => classes.AssignableTo(typeof(IStateMachineConfigurator<,,>)))
                   .UsingRegistrationStrategy(RegistrationStrategy.Append)
                   .AsImplementedInterfaces()
                   //.As(t => t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStateMachineConfigurator<,,>)))
                   .WithTransientLifetime()
                   .AddClasses(classes => classes.AssignableTo(typeof(IStateflowProvider<,,,>)))
                   .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                   .AsImplementedInterfaces()
                   //.As(t => t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStateflowProvider<,,,>)))
                   .WithTransientLifetime());

            return services;
        }
    }
}
