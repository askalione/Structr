using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Stateflows;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides <see cref="IServiceCollection"/> extensions for registering stateflow services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds stateflow services and registers all configurations placed in provided assemblies.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="assembliesToScan">Assemblies to search configurations in.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure services.</returns>
        public static IServiceCollection AddStateflows(this IServiceCollection services, params Assembly[] assembliesToScan)
            => AddStateflows(services, null, assembliesToScan);

        /// <summary>
        /// Adds stateflow services and registers all configurations placed in provided assemblies.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configureOptions">An <see cref="Action{StateflowServiceOptions}"/> to configure stateflows.</param>
        /// <param name="assembliesToScan">Assemblies to search configurations in.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure services.</returns>
        public static IServiceCollection AddStateflows(this IServiceCollection services,
            Action<StateflowServiceOptions> configureOptions,
            params Assembly[] assembliesToScan)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var options = new StateflowServiceOptions();

            configureOptions?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(typeof(IStateMachineProvider), options.ProviderType, options.ProviderServiceLifetime));

            services.AddClasses(assembliesToScan);

            return services;
        }

        private static IServiceCollection AddClasses(this IServiceCollection services, params Assembly[] assembliesToScan)
        {
            if (assembliesToScan != null && assembliesToScan.Length > 0)
            {
                var allTypes = assembliesToScan
                    .Where(a => !a.IsDynamic && a != typeof(IStateMachineProvider).Assembly)
                    .Distinct()
                    .SelectMany(a => a.DefinedTypes)
                    .ToArray();

                var openTypes = new[]
                {
                    new { Type = typeof(IStateMachineConfigurator<,,>), SkipIfExists = false },
                    new { Type = typeof(IStateflowProvider<,,,>), SkipIfExists = true }
                };

                foreach (var typeRegistration in openTypes.SelectMany(openType => allTypes
                    .Select(t => new { Type = t, SkipIfExists = openType.SkipIfExists })
                    .Where(t => t.Type.IsClass
                        && !t.Type.IsGenericType
                        && !t.Type.IsAbstract
                        && t.Type.AsType().ImplementsGenericInterface(openType.Type))))
                {
                    var implementationType = typeRegistration.Type.AsType();
                    foreach (var interfaceType in implementationType.GetInterfaces()
                        .Where(i => openTypes.Any(openType => i.ImplementsGenericInterface(openType.Type))))
                    {
                        if (typeRegistration.SkipIfExists)
                        {
                            services.TryAddTransient(interfaceType, implementationType);
                        }
                        else
                        {
                            services.AddTransient(interfaceType, implementationType);
                        }
                    }
                }
            }

            return services;
        }

        private static bool ImplementsGenericInterface(this Type type, Type interfaceType)
            => type.IsGenericType(interfaceType) || type.GetTypeInfo().ImplementedInterfaces.Any(@interface => @interface.IsGenericType(interfaceType));

        private static bool IsGenericType(this Type type, Type genericType)
            => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }
}
