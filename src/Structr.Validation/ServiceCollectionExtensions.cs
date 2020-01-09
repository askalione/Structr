using Microsoft.Extensions.DependencyInjection.Extensions;
using Scrutor;
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
                throw new ArgumentNullException(nameof(services));
            if (assembliesToScan == null)
                throw new ArgumentNullException(nameof(assembliesToScan));
            if (!assembliesToScan.Any())
                throw new ArgumentException("No assemblies found to scan. At least one assembly to scan for validators is required");

            var options = new ValidationServiceOptions();

            configureOptions?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(typeof(IValidationProvider), options.ProviderType, options.Lifetime));

            services
                .Scan(opt => opt.FromAssemblies(assembliesToScan)
                    .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsImplementedInterfaces()
                    //.As(c => c.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
                    .WithScopedLifetime());

            return services;
        }
    }
}
