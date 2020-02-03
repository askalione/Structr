using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;

namespace Structr.EntityFramework.Options
{
    public class ValueObjectConfigurationOptions
    {
        public Action<ConventionTypeConfiguration> Configure { get; set; }
        public Func<ConventionTypeConfiguration, PropertyInfo, string> ScalarPropertyNameFactory { get; set; }

        public ValueObjectConfigurationOptions()
        {
            ScalarPropertyNameFactory = (typeConfiguration, prop) => prop.Name;
        }
    }
}
