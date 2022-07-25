#if NET6_0
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Structr.Abstractions.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="DateOnly"/> to or from JSON.
    /// </summary>
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private readonly string _serializationFormat;

        /// <summary>
        /// Initializes an instance of <see cref="DateOnlyJsonConverter"/>.
        /// </summary>
        /// <param name="serializationFormat">Serialization format. Default value "yyyy-MM-dd" according to the ISO 8601 standard.</param>
        public DateOnlyJsonConverter(string serializationFormat = null)
        {
            _serializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(_serializationFormat));
    }
}
#endif
