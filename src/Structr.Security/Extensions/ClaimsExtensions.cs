using Structr.Security.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Structr.Security.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{Claim}"/>.
    /// </summary>
    public static class ClaimsExtensions
    {
        /// <summary>
        /// Retrieves first claim that is matched by the specified claim type.
        /// </summary>
        /// <param name="source">Collection of <see cref="Claim"/> to find claim from.</param>
        /// <param name="type">The claim type to match (case insensitive).</param>
        /// <returns>The first matching claim or <see langword="null"/> if no match is found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/> and <paramref name="type"/> is <see langword="null"/> or empty.</exception>
        public static Claim FindFirst(this IEnumerable<Claim> source, string type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            Claim claim = source.FirstOrDefault(x => string.Equals(x.Type, type, StringComparison.OrdinalIgnoreCase));
            return claim;
        }

        /// <summary>
        /// Retrieves first claim value that is matched by the specified claim type.
        /// </summary>
        /// <param name="source">Collection of <see cref="Claim"/> to find claim from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The first matching claim value or <see langword="null"/> if no match is found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/> and <paramref name="type"/> is <see langword="null"/> or empty.</exception>
        public static string FindFirstValue(this IEnumerable<Claim> source, string type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            Claim claim = source.FindFirst(type);
            return claim?.Value;
        }

        /// <summary>
        /// Retrieves all of the claims values that are matched by the specified claim type.
        /// </summary>
        /// <param name="source">Collection of <see cref="Claim"/> to find claim from.</param>
        /// <param name="type">The claim type to match (case insensitive).</param>
        /// <returns>The values of matching claims.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/> and <paramref name="type"/> is <see langword="null"/> or empty.</exception>
        public static IEnumerable<string> FindAllValues(this IEnumerable<Claim> source, string type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var claims = source
                .Where(x => string.Equals(x.Type, type, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value)
                .ToList();

            return claims;
        }

        /// <summary>
        /// Retrieves value of the first claim with the specified claim type and casts it to <typeparamref name="T"/>.
        /// Throws an exception when claim not found or claim value cast is invalid.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="source">Collection of <see cref="Claim"/> to find claim from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The value of the first matching claim.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="type"/> is null.</exception>
        /// <exception cref="InvalidOperationException">If claim with specified type <paramref name="type"/> not found.</exception>
        public static T GetFirstValue<T>(this IEnumerable<Claim> source, string type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            string claimValue = source.FindFirstValue(type);
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
        /// <param name="source">Collection of <see cref="Claim"/> to find claim from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <param name="value">The value of the first matching claim.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was cast successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetFirstValue<T>(this IEnumerable<Claim> source, string type, out T value)
        {
            value = default(T);
            try
            {
                value = source.GetFirstValue<T>(type);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves values of claims with the specified claim type and casts them to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="source">Collection of <see cref="Claim"/> to find claim from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The values of matching claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="type"/> is null.</exception>
        public static IEnumerable<T> FindAllValues<T>(this IEnumerable<Claim> source, string type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            IEnumerable<string> claimValues = source.FindAllValues(type);
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
        /// Determines whether this collection of claims has a claim that is matched by the specified claim type.
        /// </summary>
        /// <param name="source">Collection of <see cref="Claim"/> to find claim from.</param>
        /// <param name="type">The claim type to match.</param>
        /// <returns><see langword="true"/> if a matching claim exists; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="type"/> is null.</exception>
        public static bool HasClaim(this IEnumerable<Claim> source, string type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var claim = source.FindFirst(type);
            return claim != null;
        }
    }
}
