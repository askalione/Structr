using Microsoft.AspNetCore.Mvc;
using Structr.Abstractions.Json.Converters;
using System;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="JsonOptions"/>.
    /// </summary>
    public static class JsonOptionsExtensions
    {
        /// <summary>
        /// Adds <see cref="DateOnly"/> and <see cref="TimeOnly"/> serializers to System.Text.Json.
        /// </summary>
        public static JsonOptions UseDateOnlyTimeOnlyConverters(this JsonOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
            return options;
        }
    }
}
