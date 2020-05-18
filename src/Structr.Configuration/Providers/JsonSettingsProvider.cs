using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Structr.Configuration.Json;
using System;
using System.IO;

namespace Structr.Configuration.Providers
{
    public class JsonSettingsProvider<TSettings> : ISettingsProvider<TSettings> where TSettings : class, new()
    {
        private readonly string _path;
        private DateTime? _lastModifiedTime;

        public JsonSettingsProvider(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            _path = path;
        }

        public TSettings GetSettings()
        {
            CheckExistsPathOrThrow(_path);

            using (StreamReader streamReader = new StreamReader(_path))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    return JObject.Load(reader).ToObject<TSettings>(GetSerializer());
                }
            }
        }

        public void SetSettings(TSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            CheckExistsPathOrThrow(_path);

            using (StreamWriter streamWriter = new StreamWriter(_path))
            {
                using (JsonTextWriter writer = new JsonTextWriter(streamWriter))
                {
                    writer.Formatting = Formatting.Indented;
                    JObject.FromObject(settings, GetSerializer())
                    .WriteTo(writer);
                }
            }
        }

        public bool IsSettingsChanged()
        {
            CheckExistsPathOrThrow(_path);

            var fileInfo = new FileInfo(_path);
            var lastModifiedTime = fileInfo.LastWriteTime;
            var isChanged = _lastModifiedTime != lastModifiedTime;
            _lastModifiedTime = lastModifiedTime;
            return isChanged;
        }

        private static JsonSerializer GetSerializer()
            => new JsonSerializer { ContractResolver = new JsonSettingsContractResolver() };

        private static void CheckExistsPathOrThrow(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Settings file not found.", path);
        }
    }
}
