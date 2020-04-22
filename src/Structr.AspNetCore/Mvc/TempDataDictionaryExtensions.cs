using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Structr.AspNetCore.Mvc
{
    public static class TempDataDictionaryExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object data;
            tempData.TryGetValue(key, out data);
            return data == null ? null : JsonConvert.DeserializeObject<T>(data.ToString());
        }
    }
}
