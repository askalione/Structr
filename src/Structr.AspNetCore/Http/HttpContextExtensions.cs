using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structr.AspNetCore.Http
{
    /// <summary>
    /// Defines extension methods on <see cref="HttpContext"/>.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets IP-address common human-readable representation for remote target.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Human-readable IP-address of remoute target.</returns>
        /// <exception cref="ArgumentNullException">When context is null.</exception>
        public static string GetIpAddress(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Connection.RemoteIpAddress.ToString();
        }

        /// <summary>
        /// Gets available authentication schemes.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of available authentication schemes.</returns>
        /// <exception cref="ArgumentNullException">When context is null.</exception>
        public static async Task<IEnumerable<AuthenticationScheme>> GetAuthenticationSchemesAsync(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var schemeProvider = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

            var allSchemes = await schemeProvider.GetAllSchemesAsync();
            //var schemes = allSchemes.Where(x => string.IsNullOrEmpty(x.DisplayName) == false);

            return allSchemes;
        }

        /// <summary>
        /// Determines whenever specified authentication scheme is available in current context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme">Name of scheme to check.</param>
        /// <returns><see langword="true"/> if authentication scheme is available, otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">When context is null.</exception>
        public static async Task<bool> IsSupportedAuthenticationSchemeAsync(this HttpContext context, string scheme)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var schemes = await GetAuthenticationSchemesAsync(context);
            var isSupported = schemes.Any(x => string.Equals(x.Name, scheme, StringComparison.OrdinalIgnoreCase));

            return isSupported;
        }
    }
}
