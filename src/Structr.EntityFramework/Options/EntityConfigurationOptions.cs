using System;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Structr.EntityFramework.Options
{
    /// <summary>
    /// Defines a set of options for configuring Entities.
    /// </summary>
    public class EntityConfigurationOptions
    {
        /// <summary>
        /// Delegate for configuring Entities.
        /// </summary>
        public Action<ConventionTypeConfiguration> Configure { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="EntityConfigurationOptions"/> with default values.
        /// </summary>
        public EntityConfigurationOptions()
        { }
    }
}
