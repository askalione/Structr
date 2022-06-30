using Structr.Abstractions;
using System.Text.RegularExpressions;

namespace Structr.Samples.EntityFrameworkCore.WebApp.Utils.Extensions
{
    public static class StringExtensions
    {
        private static Regex _isCyrillicRegex = new Regex(@"\p{IsCyrillic}", RegexOptions.Compiled);

        public static bool IsCyrillic(this string value)
        {
            Ensure.NotEmpty(value, nameof(value));

            var isCyrillic = _isCyrillicRegex.IsMatch(value);
            return isCyrillic;
        }

        private static Regex _isLatinRegex = new Regex(@"[^\p{IsCyrillic}\p{P}\d\s]+", RegexOptions.Compiled);

        public static bool IsLatin(this string value)
        {
            Ensure.NotEmpty(value, nameof(value));

            var isLatin = _isLatinRegex.IsMatch(value);
            return isLatin;
        }
    }
}
