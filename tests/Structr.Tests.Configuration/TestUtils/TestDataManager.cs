using Structr.Configuration;
using Structr.Configuration.Providers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Structr.Tests.Configuration.TestUtils
{
    internal static class TestDataManager
    {
        public static async Task<string> GetJsonAsync(string fileName)
        {
            return await File.ReadAllTextAsync(fileName);
        }

        public static async Task<JsonSettingsProvider<TestSettings>> GetSettingsJsonProviderAsync(string fileName, bool cache, params (string Name, string Value)[] data)
        {
            var options = new SettingsProviderOptions();
            options.Cache = cache;
            var path = string.IsNullOrEmpty(fileName) == false ? await GenerateJsonFileAsync(fileName, data) : "null";
            return new JsonSettingsProvider<TestSettings>(options, path);
        }

        public static async Task<string> GenerateJsonFileAsync(string fileName, params (string Name, string Value)[] data)
        {
            fileName = TestDataPath.CombineWithTemp(fileName + ".json");
            var result = "{" + string.Join(",", data.Select(x => $"\"{x.Name}\": {x.Value}")) + "}";
            await File.WriteAllTextAsync(fileName, result);
            return fileName;
        }

        public static async Task<string> GetXmlAsync(string fileName)
        {
            return await File.ReadAllTextAsync(fileName);
        }

        public static async Task<XmlSettingsProvider<TestSettings>> GetSettingsXmlProviderAsync(string fileName, bool cache, params (string Name, string Value)[] data)
        {
            var options = new SettingsProviderOptions();
            options.Cache = cache;
            var path = string.IsNullOrEmpty(fileName) == false ? await GenerateXmlFileAsync(fileName, data) : "null";
            return new XmlSettingsProvider<TestSettings>(options, path);
        }

        public static async Task<string> GenerateXmlFileAsync(string fileName, params (string Name, string Value)[] data)
        {
            fileName = TestDataPath.CombineWithTemp(fileName + ".json");
            var result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                + "<settings>"
                + string.Join("", data.Select(x => $"<{x.Name}>{x.Value}</{x.Name}>"))
                + "</settings>";
            await File.WriteAllTextAsync(fileName, result);
            return fileName;
        }
    }
}
