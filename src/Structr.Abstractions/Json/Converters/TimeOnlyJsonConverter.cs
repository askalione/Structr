#if NET6_0
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Structr.Abstractions.Json.Converters
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private readonly string serializationFormat;

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
