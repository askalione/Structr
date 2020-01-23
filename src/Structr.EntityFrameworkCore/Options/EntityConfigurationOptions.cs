using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Structr.EntityFrameworkCore.Options
{
    public class EntityConfigurationOptions
    {
        public Action<IMutableEntityType, EntityTypeBuilder> Configure { get; set; }

        public EntityConfigurationOptions()
        {

        }
    }
}
