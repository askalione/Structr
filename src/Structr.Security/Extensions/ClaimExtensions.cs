using Structr.Security.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Structr.Security.Extensions
{
    public static class ClaimExtensions
    {
        public static T GetValue<T>(this IEnumerable<Claim> src, string type)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));

            var claim = src.FirstOrDefault(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

            return claim != null
                ? claim.Value.Cast<T>()
                : default(T);
        }

        public static IEnumerable<T> GetValues<T>(this IEnumerable<Claim> src, string type)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));

            var claims = src.Where(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

            var values = new List<T>();

            if (claims.Count() > 0)
            {
                foreach (var claim in claims)
                {
                    var value = claim.Value.Cast<T>();
                    if (value != null)
                    {
                        values.Add(value);
                    }
                }
            }

            return values;
        }
    }
}
