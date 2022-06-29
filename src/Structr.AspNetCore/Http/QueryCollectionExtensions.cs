using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Structr.AspNetCore.Http
{
    /// <summary>
    /// Defines extension methods on <see cref="IQueryCollection"/>.
    /// </summary>
    public static class QueryCollectionExtensions
    {
        /// <summary>
        /// Creates an instance of <see cref="RouteValueDictionary"/> containing key-value pairs form
        /// specified <see cref="IQueryCollection"/>.
        /// </summary>
        /// <param name="collection">The <see cref="IQueryCollection"/>.</param>
        /// <returns>The <see cref="RouteValueDictionary"/> object.</returns>
        public static RouteValueDictionary ToRouteValueDictionary(this IQueryCollection collection)
        {
            var routeValues = new RouteValueDictionary();
            foreach (string key in collection.Keys)
            {
                routeValues[key] = collection[key];
            }
            return routeValues;
        }

        /// <summary>
        /// Creates an instance of <see cref="RouteValueDictionary"/> containing key-value pairs form
        /// specified <see cref="IQueryCollection"/> and append new value with specified key.
        /// </summary>
        /// <param name="collection">The <see cref="IQueryCollection"/>.</param>
        /// <param name="newKey">A key to append to.</param>
        /// <param name="newValue">A value to append to.</param>
        /// <returns>The <see cref="RouteValueDictionary"/> object.</returns>
        public static RouteValueDictionary ToRouteValueDictionary(this IQueryCollection collection, string newKey, object newValue)
        {
            // TODO: Fix somehow or refactor
            RouteValueDictionary routeValues = collection.ToRouteValueDictionary();

            routeValues.Remove(newKey);

            if (newValue != null && string.IsNullOrEmpty(newValue.ToString()) == false)
            {
                routeValues[newKey] = newValue;
            }

            return routeValues;
        }
    }
}
