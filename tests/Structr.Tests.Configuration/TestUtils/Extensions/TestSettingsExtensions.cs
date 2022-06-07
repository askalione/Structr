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
            string json = @"

{
  ""FilePath"": ""X:\\readme.txt"",
  ""SomeOwnerNameAlias"": ""Owner name"",
  ""HelpUrl"": ""hhhh""
}

";
            File.WriteAllText(path, json.Trim());
        }

        public static void WriteToXml(this TestSettings settings)
        {
            var path = TestDataPath.Combine("settings.xml");
            var xml = @"

<?xml version=""1.0"" encoding=""utf-8""?>
<settings>
  <FilePath>X:\readme.txt</FilePath>
  <SomeOwnerNameAlias>Owner name</SomeOwnerNameAlias>
  <HelpUrl />
  <ApiKey />
</settings>
                  ";
            File.WriteAllText(path, xml.Trim());
        }
    }
}
