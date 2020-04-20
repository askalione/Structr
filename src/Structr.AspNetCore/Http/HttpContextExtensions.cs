using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structr.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static string GetIpAddress(this HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.Connection.RemoteIpAddress.ToString();
        }

        public static async Task<IEnumerable<AuthenticationScheme>> GetAuthenticationSchemesAsync(this HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var schemeProvider = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

            var allSchemes = await schemeProvider.GetAllSchemesAsync();
            var schemes = allSchemes.Where(x => !string.IsNullOrEmpty(x.DisplayName));

            return schemes;
        }

        public static async Task<bool> IsSupportedAuthenticationSchemeAsync(this HttpContext context, string scheme)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var schemes = await GetAuthenticationSchemesAsync(context);
            var isSupported = schemes.Any(x => string.Equals(x.Name, scheme, StringComparison.OrdinalIgnoreCase));

            return isSupported;
        }
    }
}
