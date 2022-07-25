using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System;

namespace Structr.Configuration.Providers
{
    /// <summary>
    /// Provides functionality for access to a JSON file with settings <typeparamref name="TSettings"/>.
    /// </summary>
    public class JsonSettingsProvider<TSettings> : FileSettingsProvider<TSettings>
        where TSettings : class, new()
    {
        /// <summary>
        /// Initializes a new <see cref="JsonSettingsProvider{TSettings}"/> instance.
        /// </summary>
        /// <param name="path">The path to JSON file with settings.</param>
        /// <param name="options">The options object to make additional configurations.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public JsonSettingsProvider(string path, SettingsProviderOptions options)
            : base(path, options)
        {
        }

        /// <summary>
        /// Load settings from JSON file.
        /// </summary>
        /// <returns>Loaded settings.</returns>
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

        /// <summary>
        /// Update settings in JSON file.
        /// </summary>
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
