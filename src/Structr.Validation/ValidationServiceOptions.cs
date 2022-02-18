using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Validation
{
    public class ValidationServiceOptions
    {
        public Type ProviderType { get; set; }
        public ServiceLifetime Lifetime { get; set; }

        public ValidationServiceOptions()
        {
            ProviderType = typeof(ValidationProvider);
            Lifetime = ServiceLifetime.Scoped;
        }
    }
}
