using Structr.Security.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Structr.Security.Extensions
{
    public static class ClaimExtensions
    {
        public static ClaimsIdentity AddClaim(this ClaimsIdentity identity, string type, string value)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            identity.AddClaim(new Claim(type, value));
            return identity;
        }

        public static ClaimsIdentity SetClaim(this ClaimsIdentity identity, string type, string value)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            identity.RemoveClaims(type);

            if (string.IsNullOrEmpty(value) == false)
            {
                identity.AddClaim(type, value);
            }

            return identity;
        }

        public static ClaimsIdentity SetClaims(this ClaimsIdentity identity, string type, IEnumerable<string> values)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            identity.RemoveClaims(type);

            foreach (var value in values.Distinct(StringComparer.Ordinal))
            {
                identity.AddClaim(type, value);
            }

            return identity;
        }

        public static ClaimsIdentity RemoveClaims(this ClaimsIdentity identity, string type)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            foreach (var claim in identity.FindAll(type).ToList())
            {
                identity.RemoveClaim(claim);
            }

            return identity;
        }

        public static ClaimsPrincipal SetClaim(this ClaimsPrincipal principal, string type, string value)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            principal.RemoveClaims(type);

            if (string.IsNullOrEmpty(value) == false)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(type, value);
            }

            return principal;
        }

        public static ClaimsPrincipal SetClaims(this ClaimsPrincipal principal, string type, IEnumerable<string> values)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            principal.RemoveClaims(type);

            foreach (var value in values.Distinct(StringComparer.Ordinal))
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(type, value);
            }

            return principal;
        }

        public static ClaimsPrincipal RemoveClaims(this ClaimsPrincipal principal, string type)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            foreach (var identity in principal.Identities)
            {
                foreach (var claim in identity.FindAll(type).ToList())
                {
                    identity.RemoveClaim(claim);
                }
            }

            return principal;
        }

        public static T GetClaim<T>(this ClaimsPrincipal principal, string type)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var value = principal.FindAll(type).GetValue<T>(type);

            return value;
        }

        public static IEnumerable<T> GetClaims<T>(this ClaimsPrincipal principal, string type)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var claims = principal.FindAll(type);
            var values = claims.Select(x => x.GetValue<T>()).ToList();

            return values;
        }

        public static bool HasClaim(this ClaimsPrincipal principal, string type)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var hasClaim = principal.FindAll(type).Any();

            return hasClaim;
        }

        public static T GetValue<T>(this IEnumerable<Claim> source, string type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var claim = source.FirstOrDefault(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

            return claim != null ? claim.GetValue<T>() : default(T);
        }

        public static IEnumerable<T> GetValues<T>(this IEnumerable<Claim> source, string type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var claims = source.Where(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

            var values = claims.Select(x => x.GetValue<T>()).ToList();

            return values;
        }

        public static T GetValue<T>(this Claim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var value = claim.Value.Cast<T>();

            return value;
        }
    }
}
