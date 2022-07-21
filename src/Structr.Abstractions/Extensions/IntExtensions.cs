using System;

namespace Structr.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="int"/>.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Creates a human readable kilo format string from <see cref="int"/> value.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <returns>String with human readable kilo format.</returns>
        public static string ToKiloFormatString(this int value)
        {
            if (value >= 100000000)
            {
                return (value / 1000000D).ToString("#,0M");
            }

            if (value >= 10000000)
            {
                return (value / 1000000D).ToString("0.#M");
            }

            if (value >= 100000)
            {
                return (value / 1000D).ToString("#,0K");
            }

            if (value >= 1000)
            {
                return (value / 1000D).ToString("0.#K");
            }

            return value.ToString("#,0");
        }

        /// <summary>
        /// Creates a human readable plural form from <see cref="int"/> value.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="oneForm">Single item form.</param>
        /// <param name="twoForm">Two item form</param>
        /// <param name="manyForm">Many item form.</param>
        /// <returns>String with human readable plural form of <paramref name="value"/>.</returns>
        public static string ToPluralFormString(this int value,
            string oneForm,
            string twoForm,
            string manyForm)
        {
            Ensure.NotEmpty(oneForm, nameof(oneForm));
            Ensure.NotEmpty(twoForm, nameof(twoForm));
            Ensure.NotEmpty(manyForm, nameof(manyForm));
            return ToPluralFormString(value, new string[] { oneForm, twoForm, manyForm });
        }

        private static string ToPluralFormString(int value, string[] forms)
        {
            int num = Math.Abs(value);
            int[] cases = new int[] { 2, 0, 1, 1, 1, 2 };
            return forms[(num % 100 > 4 && num % 100 < 20) ? 2 : cases[Math.Min(num % 10, 5)]];
        }
    }
}
