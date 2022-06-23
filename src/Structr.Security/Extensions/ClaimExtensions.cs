using Structr.Security.Internal;
using System;
using System.Security.Claims;

namespace Structr.Security.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Claim"/>.
    /// </summary>
    public static class ClaimExtensions
    {
        /// <summary>
        /// Retrieves value of claim and casts it to <typeparamref name="T"/>.
        /// Throws an exception when claim value cast is invalid.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="claim">The <see cref="Claim"/>.</param>
        /// <returns>Value of claim.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="claim"/> is null.</exception>
        public static T GetValue<T>(this Claim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            T value = claim.Value.CastClaimValueOrThrow<T>();

            return value;
        }

        /// <summary>
        /// Retrieves value of claim and casts it to <typeparamref name="T"/>.
        /// A return value indicates whether the cast value succeeded.
        /// </summary>
        /// <typeparam name="T">Type of claim value.</typeparam>
        /// <param name="claim">The <see cref="Claim"/>.</param>
        /// <param name="value">Value of claim.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was cast successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetValue<T>(this Claim claim, out T value)
        {
            value = default(T);
            try
            {
                value = claim.GetValue<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
