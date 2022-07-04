using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.AspNetCore.Routing
{
    /// <summary>
    /// Defines extension methods on <see cref="MvcOptions"/>.
    /// </summary>
    public static class RoutingMvcOptionsExtensions
    {
        /// <summary>
        /// Adds route convention with <see cref="SlugifyParameterTransformer"/>.
        /// </summary>
        /// <param name="options">The <see cref="MvcOptions"/>.</param>
        /// <returns>The <see cref="MvcOptions"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        public static MvcOptions AddSlugifyRouting(this MvcOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));

            return options;
        }
    }
}
