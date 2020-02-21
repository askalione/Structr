using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Structr.AspNetCore.Mvc
{
    public static class TempDataDictionaryExtensions
    {
        public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            var data = tempData.Peek(key);
            return data == null ? null : JsonConvert.DeserializeObject<T>(data.ToString());
        }
    }
}
