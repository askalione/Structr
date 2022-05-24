using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Structr.Configuration.Internal
{
    /// <summary>
    /// Class for encrypting and decrypting text.
    /// </summary>
    internal static class StringEncryptor
    {
        /// <summary>
        /// Encrypt text using the specified password.
        /// </summary>
        /// <param name="input">Text to be encrypted.</param>
        /// <param name="passphrase">Encryption password.</param>
        /// <returns>Encrypted text.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="input"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="passphrase"/> is <see langword="null"/> or empty.</exception>
        public static string Encrypt(string input, string passphrase)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("The text must have valid value.", nameof(input));
            }
            if (string.IsNullOrEmpty(passphrase))
            {
                throw new ArgumentNullException("Key must have valid value.", nameof(passphrase));
            }

            var buffer = Encoding.UTF8.GetBytes(input);
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(passphrase)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                aes.Key = aesKey;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var plainStream = new MemoryStream(buffer))
                            {
                                plainStream.CopyTo(aesStream);
                            }
                        }

                        var resultArray = resultStream.ToArray();
                        var combined = new byte[aes.IV.Length + resultArray.Length];
                        Array.ConstrainedCopy(aes.IV, 0, combined, 0, aes.IV.Length);
                        Array.ConstrainedCopy(resultArray, 0, combined, aes.IV.Length, resultArray.Length);

                        var result = Convert.ToBase64String(combined);
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt text using the specified password.
        /// </summary>
        /// <param name="input">Encrypted text to be decrypted.</param>
        /// <param name="passphrase">Encryption password.</param>
        /// <returns>Decrypted text.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="input"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="passphrase"/> is <see langword="null"/> or empty.</exception>
        public static string Decrypt(string input, string passphrase)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("The encrypted text must have valid value.", nameof(input));
            }
            if (string.IsNullOrEmpty(passphrase))
            {
                throw new ArgumentNullException("Key must have valid value.", nameof(passphrase));
            }

            var combined = Convert.FromBase64String(input);
            var buffer = new byte[combined.Length];
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(passphrase)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                aes.Key = aesKey;

                var iv = new byte[aes.IV.Length];
                var ciphertext = new byte[buffer.Length - iv.Length];

                Array.ConstrainedCopy(combined, 0, iv, 0, iv.Length);
                Array.ConstrainedCopy(combined, iv.Length, ciphertext, 0, ciphertext.Length);

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        using (var aesStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Write))
                        {
                            using (var plainStream = new MemoryStream(ciphertext))
                            {
                                plainStream.CopyTo(aesStream);
                            }
                        }

                        var result = Encoding.UTF8.GetString(resultStream.ToArray());
                        return result;
                    }
                }
            }
        }
    }
}