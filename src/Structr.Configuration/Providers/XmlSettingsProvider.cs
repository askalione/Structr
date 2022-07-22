using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System;

namespace Structr.Configuration.Providers
{
    /// <summary>
    /// Provides functionality for access to a XML file with settings <typeparamref name="TSettings"/>.
    /// </summary>
    public class XmlSettingsProvider<TSettings> : FileSettingsProvider<TSettings> where TSettings : class, new()
    {
        /// <summary>
        /// Initializes a new <see cref="XmlSettingsProvider{TSettings}"/> instance.
        /// </summary>
        /// <param name="options">The options object to make additional configurations.</param>
        /// <param name="path">The path to XML file with settings.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public XmlSettingsProvider(SettingsProviderOptions options, string path)
            : base(options, path)
        {
        }

        /// <summary>
        /// Load settings from XML file.
        /// </summary>
        /// <returns>Loaded settings.</returns>
        protected override TSettings LoadSettings()
        {
            ValidatePathOrThrow();

            var xml = XDocument.Load(Path);
            xml.Declaration = null;
            var json = JsonConvert.SerializeXNode(xml, Formatting.None, true);
            var jobj = JObject.Parse(json);
            var settings = jobj.ToObject<TSettings>(JsonSerializer);

            return settings;
        }

        /// <summary>
        /// Update settings in XML file.
        /// </summary>
        protected override void UpdateSettings(TSettings settings)
        {
            ValidatePathOrThrow();

            var jobj = JObject.FromObject(settings, JsonSerializer);
            var json = jobj.ToString();
            var xmlRootName = System.IO.Path.GetFileNameWithoutExtension(Path);
            var xml = JsonConvert.DeserializeXNode(json, xmlRootName);
            xml.Save(Path);
        }
    }
}
