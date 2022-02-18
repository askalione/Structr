using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Validation;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services, params Assembly[] assembliesToScan)
            => AddValidation(services, null, assembliesToScan);

        public static IServiceCollection AddValidation(this IServiceCollection services,
            Action<ValidationServiceOptions> configureOptions,
            params Assembly[] assembliesToScan)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var options = new ValidationServiceOptions();

            configureOptions?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(typeof(IValidationProvider), options.ProviderType, options.Lifetime));

            services.AddClasses(assembliesToScan);

            return services;
        }

        private static IServiceCollection AddClasses(this IServiceCollection services, params Assembly[] assembliesToScan)
        {
            if (assembliesToScan != null && assembliesToScan.Length > 0)
            {
                var allTypes = assembliesToScan
                    .Where(a => !a.IsDynamic && a != typeof(IValidationProvider).Assembly)
                    .Distinct()
                    .SelectMany(a => a.DefinedTypes)
                    .ToArray();

                var openTypes = new[]
                {
                    typeof(IValidator<>)
                };

                foreach (var typeInfo in openTypes.SelectMany(openType => allTypes
                    .Where(t => t.IsClass
                        && !t.IsGenericType
                        && !t.IsAbstract
                        && t.AsType().ImplementsGenericInterface(openType))))
                {
                    var implementationType = typeInfo.AsType();

                    foreach (var interfaceType in implementationType.GetInterfaces()
                        .Where(i => openTypes.Any(openType => i.ImplementsGenericInterface(openType))))
                    {
                        services.TryAddTransient(interfaceType, implementationType);
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
