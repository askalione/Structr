using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Structr.Configuration.Json;
using System;
using System.IO;
using System.Xml.Linq;

namespace Structr.Configuration.Providers
{
    public class XmlSettingsProvider<TSettings> : ISettingsProvider<TSettings> where TSettings : class, new()
    {
        private readonly string _path;
        private DateTime? _lastModifiedTime;

        public XmlSettingsProvider(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            _path = path;
        }

        public TSettings GetSettings()
        {
            CheckExistsPathOrThrow(_path);

            var xml = XDocument.Load(_path);
            xml.Declaration = null;
            return JObject.Parse(JsonConvert.SerializeXNode(xml, Formatting.None, true))
                .ToObject<TSettings>(GetSerializer());
        }

        public void SetSettings(TSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            CheckExistsPathOrThrow(_path);

            var xml = JsonConvert.DeserializeXNode(JObject.FromObject(settings, GetSerializer()).ToString(),
                Path.GetFileNameWithoutExtension(_path));
            xml.Save(_path);
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
