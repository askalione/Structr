using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="IQueryCollection"/>.
    /// </summary>
    public static class QueryCollectionExtensions
    {
        /// <summary>
        /// Creates an instance of <see cref="RouteValueDictionary"/> containing key-value pairs form specified <see cref="IQueryCollection"/>. 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns><see cref="RouteValueDictionary"/> object.</returns>
        public static RouteValueDictionary ToRouteValueDictionary(this IQueryCollection collection)
        {
            var routeValues = new RouteValueDictionary();
            foreach (string key in collection.Keys)
            {
                routeValues[key] = collection[key];
            }
            return routeValues;
        }

        // TODO: some refactoring
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
