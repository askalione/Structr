using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="ITempDataDictionary"/>.
    /// </summary>
    public static class TempDataDictionaryExtensions
    {
        /// <summary>
        /// Place a specified object in current <see cref="ITempDataDictionary"/> serializing it. Overwrites an existing value if needed.
        /// </summary>
        /// <typeparam name="T">Type of object to be placed into dictionary.</typeparam>
        /// <param name="tempData">The <see cref="ITempDataDictionary"/></param>
        /// <param name="key">Key for object.</param>
        /// <param name="value">Object to be placed into dictionary.</param>
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Get object for specified key from current <see cref="ITempDataDictionary"/>.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="tempData">The <see cref="ITempDataDictionary"/>.</param>
        /// <param name="key">Key for object.</param>
        /// <returns>Object for specified key or <see langword="null"/> if no such key exists.</returns>
        public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object data;
            tempData.TryGetValue(key, out data);
            return data == null ? null : JsonConvert.DeserializeObject<T>(data.ToString());
        }
    }
}
