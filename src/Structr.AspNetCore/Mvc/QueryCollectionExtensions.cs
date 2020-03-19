using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Structr.AspNetCore.Mvc
{
    public static class QueryCollectionExtensions
    {
        public static RouteValueDictionary ToRouteValueDictionary(this IQueryCollection collection)
        {
            var routeValues = new RouteValueDictionary();
            foreach (string key in collection.Keys)
            {
                routeValues[key] = collection[key];
            }
            return routeValues;
        }

        public static RouteValueDictionary ToRouteValueDictionary(this IQueryCollection collection, string newKey, object newValue)
        {
            string value = newValue?.ToString();

            var routeValueDictionary = new RouteValueDictionary();
            foreach (var key in collection.Keys)
            {
                if (key == null) continue;
                if (routeValueDictionary.ContainsKey(key))
                    routeValueDictionary.Remove(key);

                routeValueDictionary.Add(key, collection[key]);
            }
            if (string.IsNullOrEmpty(value))
            {
                routeValueDictionary.Remove(newKey);
            }
            else
            {
                if (routeValueDictionary.ContainsKey(newKey))
                    routeValueDictionary.Remove(newKey);

                routeValueDictionary.Add(newKey, value);
            }
            return routeValueDictionary;
        }
    }
}
