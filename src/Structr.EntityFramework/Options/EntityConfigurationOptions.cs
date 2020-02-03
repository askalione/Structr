using System;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Structr.EntityFramework.Options
{
    public class EntityConfigurationOptions
    {
        public Action<ConventionTypeConfiguration> Configure { get; set; }

        public EntityConfigurationOptions()
        {

        }
    }
}
