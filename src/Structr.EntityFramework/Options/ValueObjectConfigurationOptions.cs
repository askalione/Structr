using System;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Structr.EntityFramework.Options
{
    /// <summary>
    /// Defines a set of options for configuring Value Objects.
    /// </summary>
    public class ValueObjectConfigurationOptions
    {
        /// <summary>
        /// Delegate for configuring Value Objects.
        /// </summary>
        public Action<ConventionTypeConfiguration> Configure { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="ValueObjectConfigurationOptions"/> with default values.
        /// </summary>
        public ValueObjectConfigurationOptions()
        {
        }
    }
}
