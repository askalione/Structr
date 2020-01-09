using System.Text.RegularExpressions;

namespace Structr.Abstractions.Helpers
{
    public static class HashHelper
    {
        /// <summary>
        /// Return new hashed string.
        /// </summary>
        /// <param name="value">Target string value.</param>
        /// <returns>
        /// Return new instance of string.
        /// </returns>
        public static string HashString(string value)
        {
            string hash = value;
            if (!string.IsNullOrWhiteSpace(hash))
            {
                hash = hash.ToLower().Trim();
                hash = hash.Replace("&quot;", " ");
                hash = hash.Replace("&nbsp;", " ");
                hash = hash.Replace("&lt;", " ");
                hash = hash.Replace("&gt;", " ");
                hash = hash.Replace("&apos;", " ");
                hash = hash.Replace("&apos;", "'");
                hash = hash.Replace("ё", "е");
                hash = hash.Replace("Ё", "Е");
                hash = Regex.Replace(hash, @"&(\w{1})\w{3,5};", "$1");
                hash = Regex.Replace(hash, @"\b(l'|d'|'s)\b", " ");
                hash = Regex.Replace(hash, @"\b'\b/", "");
                hash = Regex.Replace(hash, @" +/", " ");
                hash = Regex.Replace(hash, @"^ /", "");
                hash = Regex.Replace(hash, @" $", "");

                hash = hash.Replace(" ", "");
                hash = hash.Replace(".", "Е");
                hash = hash.Replace(",", "Е");
            }

            return hash;
        }
    }
}
