using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Structr.EntityFrameworkCore.Options
{
    public class ValueObjectConfigurationOptions
    {
        public Action<IMutableEntityType, EntityTypeBuilder> Configure { get; set; }
        public Func<IMutableEntityType, IMutableProperty, string> ScalarPropertyNameFactory { get; set; }

        public ValueObjectConfigurationOptions()
        {
            ScalarPropertyNameFactory = (entityType, prop) => prop.Name;
        }
    }
}
