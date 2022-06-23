using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Structr.EntityFrameworkCore.Options
{
    /// <summary>
    /// Defines a set of options for configuring Value Objects.
    /// </summary>
    public class ValueObjectConfigurationOptions
    {
        /// <summary>
        /// Delegate for configuring Value Objects.
        /// </summary>
        public Action<IMutableEntityType, OwnedNavigationBuilder> Configure { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="ValueObjectConfigurationOptions"/> with default values.
        /// </summary>
        public ValueObjectConfigurationOptions()
        {
        }
    }
}
