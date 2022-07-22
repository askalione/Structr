#if NET6_0
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Structr.Abstractions.Json.Converters
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private readonly string _serializationFormat;

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
