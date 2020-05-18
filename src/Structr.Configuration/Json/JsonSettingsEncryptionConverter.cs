using Newtonsoft.Json;
using Structr.Security;
using System;

namespace Structr.Configuration.Json
{
    class JsonSettingsEncryptionConverter : JsonConverter
    {
        private readonly string _passphrase;

        public JsonSettingsEncryptionConverter(string passphrase)
        {
            if (string.IsNullOrEmpty(passphrase))
                throw new ArgumentNullException(nameof(passphrase));

            _passphrase = passphrase;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
            {
                writer.WriteNull();
                return;
            }

            var encryptedValue = StringEncryptor.Encrypt(stringValue, _passphrase);
            writer.WriteValue(encryptedValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stringValue = reader.Value as string;
            if (string.IsNullOrEmpty(stringValue))
            {
                return reader.Value;
            }

            var decryptedValue = StringEncryptor.Decrypt(stringValue, _passphrase);
            return decryptedValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
