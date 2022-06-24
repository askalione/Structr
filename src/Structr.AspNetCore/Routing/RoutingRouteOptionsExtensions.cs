using Microsoft.AspNetCore.Routing;
using System;

namespace Structr.AspNetCore.Routing
{
    /// <summary>
    /// Defines extension methods on <see cref="RouteOptions"/>.
    /// </summary>
    public static class RoutingRouteOptionsExtensions
    {
        /// <summary>
        /// Adds <see cref="SlugifyParameterTransformer"/> to constraint map.
        /// </summary>
        /// <param name="options">The <see cref="RouteOptions"/>.</param>
        /// <param name="constraintKey">Key for constraint map. Default value is "slugify".</param>
        /// <returns>The <see cref="RouteOptions"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        public static RouteOptions AddSlugifyRouting(this RouteOptions options, string constraintKey = "slugify")
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (string.IsNullOrWhiteSpace(constraintKey))
            {
                throw new ArgumentNullException(nameof(constraintKey));
            }

            options.ConstraintMap.Add(constraintKey, typeof(SlugifyParameterTransformer));

            return options;
        }
    }
}
