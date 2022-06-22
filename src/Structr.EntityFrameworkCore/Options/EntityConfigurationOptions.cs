using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Structr.EntityFrameworkCore.Options
{
    /// <summary>
    /// Defines a set of options for configuring Entities.
    /// </summary>
    public class EntityConfigurationOptions
    {
        /// <summary>
        /// Delegate for configuring Entities.
        /// </summary>
        public Action<IMutableEntityType, EntityTypeBuilder> Configure { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="EntityConfigurationOptions"/> with default values.
        /// </summary>
        public EntityConfigurationOptions()
        {
        }
    }
}
