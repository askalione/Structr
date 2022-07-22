using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.TypeConverters;
using System;
using System.ComponentModel;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="MvcOptions"/>.
    /// </summary>
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// Adds <see cref="TypeConverter"/> to <see cref="DateOnly"/> and <see cref="TimeOnly"/> type definitions.
        /// </summary>
        /// <param name="options">The <see cref="MvcOptions"/>.</param>
        public static MvcOptions UseDateOnlyTimeOnlyConverters(this MvcOptions options)
        {
            TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
            TypeDescriptor.AddAttributes(typeof(TimeOnly), new TypeConverterAttribute(typeof(TimeOnlyTypeConverter)));
            return options;
        }
    }
}
