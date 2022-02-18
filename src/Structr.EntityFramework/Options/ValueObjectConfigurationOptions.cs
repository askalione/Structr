using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;

namespace Structr.EntityFramework.Options
{
    public class ValueObjectConfigurationOptions
    {
        public Action<ConventionTypeConfiguration> Configure { get; set; }

        public ValueObjectConfigurationOptions()
        {
        }
    }
}
