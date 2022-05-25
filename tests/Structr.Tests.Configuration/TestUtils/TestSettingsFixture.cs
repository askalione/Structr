using Structr.Tests.Configuration.TestUtils.Extensions;
using System;

namespace Structr.Tests.Configuration.TestUtils
{
    internal class TestSettingsFixture : IDisposable
    {
        public TestSettingsFixture()
        {
            var defaultSettings = new TestSettings();
            defaultSettings.WriteToJson();
            defaultSettings.WriteToXml();
        }

        public void Dispose()
        {
            var defaultSettings = new TestSettings();
            defaultSettings.WriteToJson();
            defaultSettings.WriteToXml();
        }
    }
}
