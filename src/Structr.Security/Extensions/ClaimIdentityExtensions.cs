using Structr.Security.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Structr.Security.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ClaimsIdentity"/>.
    /// </summary>
    public static class ClaimIdentityExtensions
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

            identity.RemoveAllClaims(type);

            if (string.IsNullOrEmpty(value) == false)
            {
                identity.AddClaim(type, value);
            }

            return identity;
        }

        /// <summary>
        /// Sets a values for claims with specified type in current identity. If <paramref name="values"/>
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

            identity.RemoveAllClaims(type);

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
        public static ClaimsIdentity RemoveAllClaims(this ClaimsIdentity identity, string type)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            List<Claim> claims = identity.FindAll(type).ToList();
            foreach (var claim in claims)
            {
                identity.RemoveClaim(claim);
            }

            return identity;
        }

        /// <summary>
        /// Retrieves first claim value that is matched by the specified claim type.
        /// </summary>
        /// <param name="identity">The identity to get claim value from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The value of the first matching claim or null if no match is found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="identity"/> is <see langword="null"/> and <paramref name="type"/> is <see langword="null"/> or empty.</exception>
        public static string FindFirstValue(this ClaimsIdentity identity, string type)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            Claim claim = identity.FindFirst(type);
            return claim?.Value;
        }

        /// <summary>
        /// Retrieves all of the claims values that are matched by the specified claim type.
        /// </summary>
        /// <param name="identity">The identity to get claims values from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The values of matching claims.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="identity"/> is <see langword="null"/> and <paramref name="type"/> is <see langword="null"/> or empty.</exception>
        public static IEnumerable<string> FindAllValues(this ClaimsIdentity identity, string type)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            IEnumerable<string> values = identity.FindAll(type)
                .Select(x => x.Value)
                .ToList();

            return values;
        }

        /// <summary>
        /// Retrieves value of the first claim with the specified claim type and casts it to <typeparamref name="T"/>.
        /// Throws an exception when claim not found or claim value cast is invalid.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="identity">The identity to get claim from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The value of the first matching claim.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="identity"/> is <see langword="null"/> and <paramref name="type"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="InvalidOperationException">If claim with specified type <paramref name="type"/> not found.</exception>
        public static T GetFirstValue<T>(this ClaimsIdentity identity, string type)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            string claimValue = identity.FindFirstValue(type);
            if (string.IsNullOrEmpty(claimValue))
            {
                throw new InvalidOperationException($"Claim with type \"{type}\" not found.");
            }
            T value = claimValue.CastClaimValueOrThrow<T>();

            return value;
        }

        /// <summary>
        /// Retrieves value of the first claim with the specified claim type and casts it to <typeparamref name="T"/>.
        /// A return value indicates whether the cast value succeeded.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="identity">The identity to get claim from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <param name="value">The value of the first matching claim.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was cast successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetFirstValue<T>(this ClaimsIdentity identity, string type, out T value)
        {
            value = default(T);
            try
            {
                value = identity.GetFirstValue<T>(type);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves all of the claims values that are matched by the specified claim type and casts it to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="identity">The identity to get claims from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The values of matching claims of specified type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="identity"/> is <see langword="null"/> and <paramref name="type"/> is <see langword="null"/> or empty.</exception>
        public static IEnumerable<T> FindAllValues<T>(this ClaimsIdentity identity, string type)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            IEnumerable<string> claimValues = identity.FindAllValues(type);
            List<T> values = new List<T>();
            foreach (var claimValue in claimValues)
            {
                if (claimValue.TryCast(out T value))
                {
                    values.Add(value);
                }
            }

            return values;
        }

        /// <summary>
        /// Determines whether this claims identity has a claim that is matched by the specified claim type.
        /// </summary>
        /// <param name="identity">The identity to find claim from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns><see langword="true"/> if a matching claim exists; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="identity"/> is <see langword="null"/> and <paramref name="type"/> is <see langword="null"/> or empty.</exception>
        public static bool HasClaim(this ClaimsIdentity identity, string type)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            Claim claim = identity.FindFirst(type);
            return claim != null;
        }
    }
}
