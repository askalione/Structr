using Structr.Security.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Structr.Security.Extensions
{
    public static class ClaimExtensions
    {
        /// <summary>
        /// Adds a single claim to this claims identity.
        /// </summary>
        /// <param name="identity">The identity to add claim.</param>
        /// <param name="type">The claim type.</param>
        /// <param name="value">The claim value.</param>
        /// <returns>The identity with added claim.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="identity"/>, <paramref name="type"/> or <paramref name="value"/> is null.</exception>
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

        /// <summary>
        /// Sets a value for claim with specified type in current identity. If <paramref name="value"/>
        /// is <see langword="null"/> then claim will be removed. If claim not existed then it will be added.
        /// </summary>
        /// <param name="identity">The identity to set claim in.</param>
        /// <param name="type">The claim type.</param>
        /// <param name="value">The claim value.</param>
        /// <returns>The identity with changed or added claim.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="value"/> is null.</exception>
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

        /// <summary>
        /// Sets a values for claims with specified type in current identity. If <paramref name="value"/>
        /// is <see langword="null"/> then claim will be removed. If claim not existed then it will be added.
        /// </summary>
        /// <param name="identity">The identity to set claim in.</param>
        /// <param name="type">The claims type.</param>
        /// <param name="values">List of claims values.</param>
        /// <returns>The identity with changed or added claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="identity"/>, <paramref name="type"/> or <paramref name="values"/> is null.</exception>
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

        /// <summary>
        /// Attempts to remove claims with specified type from the claims identity.
        /// </summary>
        /// <param name="identity">The identity to remove claims from.</param>
        /// <param name="type">Type of claims to be removed.</param>
        /// <returns>The identity with removed claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="identity"/> or <paramref name="type"/> is null.</exception>
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

        /// <summary>
        /// Sets a value for claim with specified type in current claims principal. If <paramref name="value"/>
        /// is <see langword="null"/> then claim will be removed. If claim not existed then it will be added.
        /// </summary>
        /// <param name="principal">The principal to set claim in.</param>
        /// <param name="type">The claims type.</param>
        /// <param name="value">List of claims values.</param>
        /// <returns>The principal with changed or added claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="principal"/>, <paramref name="type"/> is null.</exception>
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

        /// <summary>
        /// Sets a values for claims with specified type in current claims principal. If <paramref name="value"/>
        /// is <see langword="null"/> then claim will be removed. If claim not existed then it will be added.
        /// </summary>
        /// <param name="principal">The identity to set claim in.</param>
        /// <param name="type">The claims type.</param>
        /// <param name="values">List of claims values.</param>
        /// <returns>The principal with changed or added claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="principal"/>, <paramref name="type"/> or <paramref name="values"/> is null.</exception>
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

        /// <summary>
        /// Attempts to remove claims with specified type from the claims principal.
        /// </summary>
        /// <param name="principal">The principal to remove claims from.</param>
        /// <param name="type">Type of claims to be removed.</param>
        /// <returns>The principal with removed claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="principal"/> or <paramref name="type"/> is null.</exception>
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

        /// <summary>
        /// Retrieves value of the first claim with the specified claim type and casts it to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="principal">The principal to get claim from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The value of the first matching claim or null if no match is found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="principal"/> or <paramref name="type"/> is null.</exception>
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

        /// <summary>
        /// Retrieves values of claims with the specified claim type and casts them to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="principal">The principal to get claims from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The values of matching claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="principal"/> or <paramref name="type"/> is null.</exception>
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

        /// <summary>
        /// Determines whether any of the claims identities associated with this claims principal
        /// contains a claim with the specified claim type.
        /// </summary>
        /// <param name="principal">The principal to search claims in.</param>
        /// <param name="type">The type of the claim to match.</param>
        /// <returns><see langword="true"/> if a matching claim exists; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="principal"/> or <paramref name="type"/> is null.</exception>
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

        /// <summary>
        /// Retrieves value of the first claim with the specified claim type and casts it to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="source"></param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The value of the first matching claim or null if no match is found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="type"/> is null.</exception>
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

        /// <summary>
        /// Retrieves values of claims with the specified claim type and casts them to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="source"></param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The values of matching claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="type"/> is null.</exception>
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

        /// <summary>
        /// Retrieves value of claim and casts it to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="claim"></param>
        /// <returns>Value of claim.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="claim"/> is null.</exception>
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
