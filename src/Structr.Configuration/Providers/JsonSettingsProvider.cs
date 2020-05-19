using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Structr.Configuration.Providers
{
    public class JsonSettingsProvider<TSettings> : FileSettingsProvider<TSettings> where TSettings : class, new()
    {
        public JsonSettingsProvider(SettingsProviderOptions options, string path)
            : base(options, path)
        {
        }

        protected override TSettings LoadSettings()
        {
            ValidatePathOrThrow();

            using (StreamReader streamReader = new StreamReader(Path))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    var jobj = JObject.Load(reader);
                    var settings = jobj.ToObject<TSettings>(JsonSerializer);
                    return settings;
                }
            }
        }

        protected override void UpdateSettings(TSettings settings)
        {
            ValidatePathOrThrow();

            using (StreamWriter streamWriter = new StreamWriter(Path))
            {
                using (JsonTextWriter writer = new JsonTextWriter(streamWriter))
                {
                    writer.Formatting = Formatting.Indented;
                    var jobj = JObject.FromObject(settings, JsonSerializer);
                    jobj.WriteTo(writer);
                }
            }
        }
    }
}
