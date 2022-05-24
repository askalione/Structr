using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Structr.Configuration.Json
{
    /// <inheritdoc cref="DefaultContractResolver"/>
    public class JsonSettingsContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            PropertyInfo propertyInfo = member as PropertyInfo;
            if (propertyInfo != null)
            {
                if (property.Writable == false)
                {
                    property.Writable = propertyInfo.GetSetMethod(true) != null;
                }

                var settingAttr = propertyInfo.GetCustomAttribute<OptionAttribute>();
                if (settingAttr != null)
                {
                    if (string.IsNullOrWhiteSpace(settingAttr.Alias) == false)
                    {
                        property.PropertyName = settingAttr.Alias;
                    }

                    property.DefaultValue = settingAttr.DefaultValue;
                    property.DefaultValueHandling = DefaultValueHandling.Populate;

                    if (propertyInfo.PropertyType == typeof(string) && !string.IsNullOrWhiteSpace(settingAttr.EncryptionPassphrase))
                    {
                        property.Converter = new JsonSettingsEncryptionConverter(settingAttr.EncryptionPassphrase);
                    }
                }
            }

            return property;
        }
    }
}
