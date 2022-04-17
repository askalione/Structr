using System;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Structr.EntityFramework
{
    public static class ConventionPrimitivePropertyConfigurationExtensions
    {
        public static ConventionPrimitivePropertyConfiguration IsRequired(this ConventionPrimitivePropertyConfiguration configuration, bool required = true)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (required)
                configuration.IsRequired();
            else
                configuration.IsOptional();

            return configuration;
        }
    }
}
