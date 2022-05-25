using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Structr.Configuration.Providers
{
    /// <summary>
    /// Provides functionality for access to a JSON file with settings <see cref="TSettings"/>.
    /// </summary>
    public class JsonSettingsProvider<TSettings> : FileSettingsProvider<TSettings> where TSettings : class, new()
    {
        /// <summary>
        /// Initializes a new <see cref="JsonSettingsProvider{TSettings}"/> instance.
        /// </summary>
        /// <param name="options">The <see cref="SettingsProviderOptions"/>.</param>
        /// <param name="path">The path to JSON file with settings.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
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
