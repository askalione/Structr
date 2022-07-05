using System;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Structr.EntityFramework
{
    /// <summary>
    /// Extensions methods for the <see cref="ConventionPrimitivePropertyConfiguration"/>.
    /// </summary>
    public static class ConventionPrimitivePropertyConfigurationExtensions
    {
        /// <summary>
        /// Applied to a property to configure the mapped database column as not nullable.
        /// </summary>
        /// <param name="configuration">The <see cref="ConventionPrimitivePropertyConfiguration"/>.</param>
        /// <param name="required"><see langword="true"/> if property is required, <see langword="false"/> otherwise.</param>
        /// <returns>The <see cref="ConventionPrimitivePropertyConfiguration"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="configuration"/> is <see langword="null"/>.</exception>
        public static ConventionPrimitivePropertyConfiguration IsRequired(this ConventionPrimitivePropertyConfiguration configuration, bool required = true)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (required)
            {
                configuration.IsRequired();
            }
            else
            {
                configuration.IsOptional();
            }

            return configuration;
        }
    }
}
