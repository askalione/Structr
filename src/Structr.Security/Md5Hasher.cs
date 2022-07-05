using System;
using System.Text;

namespace Structr.Security
{
    /// <summary>
    /// Provides functionality for hashing input strings and verifying them. Uses MD5 hash algorithm.
    /// </summary>
    public static class Md5Hasher
    {
        /// <summary>
        /// Hashes specified string using MD5 hash algorithm.
        /// </summary>
        /// <param name="input">String to be hashed.</param>
        /// <returns>Hashed string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/> is null.</exception>
        public static string Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Verifies provided input string with specified hash, using MD5 hash algorithm.
        /// </summary>
        /// <param name="hash">Hash to verify provided string with.</param>
        /// <param name="input">String be verified with string.</param>
        /// <returns><see langword="true"/> if input string correspones specified hash, otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Verify(string hash, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrEmpty(hash))
            {
                return false;
            }

            return hash.Equals(Hash(input), StringComparison.OrdinalIgnoreCase);
        }
    }
}
