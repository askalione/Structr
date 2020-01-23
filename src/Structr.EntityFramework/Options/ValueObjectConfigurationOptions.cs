using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
