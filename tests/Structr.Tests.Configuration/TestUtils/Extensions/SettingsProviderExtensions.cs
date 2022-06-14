using Structr.Configuration;

namespace Structr.Tests.Configuration.TestUtils.Extensions
{
    internal static class SettingsProviderExtensions
    {
        public static string GetPath<TSettings>(this SettingsProvider<TSettings> settingsProvider)
            where TSettings : class, new()
        {
            return settingsProvider
                .GetType()
                .GetField("Path", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(settingsProvider)
                .ToString();
        }
    }
}
