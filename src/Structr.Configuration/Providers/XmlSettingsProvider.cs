using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Structr.Configuration.Providers
{
    /// <summary>
    /// Provides functionality for access to a XML file with settings <see cref="TSettings"/>.
    /// </summary>
    /// <inheritdoc cref="FileSettingsProvider{TSettings}"/>
    public class XmlSettingsProvider<TSettings> : FileSettingsProvider<TSettings> where TSettings : class, new()
    {
        /// <summary>
        /// Initializes a new <see cref="XmlSettingsProvider{TSettings}"/> instance.
        /// </summary>
        /// <param name="options">The <see cref="SettingsProviderOptions"/>.</param>
        /// <param name="path">The path to XML file with settings.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public XmlSettingsProvider(SettingsProviderOptions options, string path)
            : base(options, path)
        {
        }

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
