using FluentAssertions;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Structr.Tests.Configuration.TestUtils.Extensions
{
    internal static class TestSettingsExtensions
    {
        public static void ShouldBeEquivalentToDefaultSettings(this TestSettings settings)
        {
            settings.Should().BeEquivalentTo(new TestSettings());
        }

        public static void WriteToJson(this TestSettings settings)
        {
            var path = TestDataPath.Combine("settings.json");
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static void WriteToXml(this TestSettings settings)
        {
            var path = TestDataPath.Combine("settings.xml");
            var serializer = new XmlSerializer(typeof(TestSettings));
            using (var fs = File.Create(path))
            {
                using (var tw = new XmlTextWriter(fs, Encoding.Unicode))
                {
                    tw.Formatting = Formatting.Indented;
                    serializer.Serialize(tw, settings);
                }
            }
        }
    }
}
