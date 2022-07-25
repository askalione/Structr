#if NET6_0
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Structr.Abstractions.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="TimeOnly"/> to or from JSON.
    /// </summary>
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private readonly string serializationFormat;

        /// <summary>
        /// Initializes an instance of <see cref="TimeOnlyJsonConverter"/>.
        /// </summary>
        /// <param name="serializationFormat">Serialization format. Default value "HH:mm:ss.fff" according to the ISO 8601 standard.</param>
        public TimeOnlyJsonConverter(string serializationFormat = null)
        {
            this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
        }

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return TimeOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
}
#endif
