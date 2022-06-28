using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Structr.Security
{
    /// <summary>
    /// Provides functionality for encrypting and decrypting strings using passphrase.
    /// </summary>
    public static class StringEncryptor
    {
        /// <summary>
        /// Encrypts input string using specified passphrase.
        /// </summary>
        /// <param name="input">String to encrypt.</param>
        /// <param name="passphrase">Passphrase to be used in encryption.</param>
        /// <returns>Encrypted string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/> or <paramref name="passphrase"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Encrypt(string input, string passphrase)
        {
            if (string.IsNullOrEmpty(passphrase))
            {
                throw new ArgumentNullException(nameof(passphrase));
            }
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            var buffer = Encoding.UTF8.GetBytes(input);
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(passphrase)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                if (aes == null)
                {
                    throw new InvalidOperationException("Aes must not be null.");
                }

                aes.Key = aesKey;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(buffer))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    var result = resultStream.ToArray();
                    var combined = new byte[aes.IV.Length + result.Length];
                    Array.ConstrainedCopy(aes.IV, 0, combined, 0, aes.IV.Length);
                    Array.ConstrainedCopy(result, 0, combined, aes.IV.Length, result.Length);

                    return Convert.ToBase64String(combined);
                }
            }
        }

        /// <summary>
        /// Decrypts input encrypted string using specified passphrase.
        /// </summary>
        /// <param name="input">String to decrypt.</param>
        /// <param name="passphrase">Passphrase to be used in decryption.</param>
        /// <returns>Decrypted string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/> or <paramref name="passphrase"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Decrypt(string input, string passphrase)
        {
            if (string.IsNullOrEmpty(passphrase))
            {
                throw new ArgumentNullException(nameof(passphrase));
            }
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            var combined = Convert.FromBase64String(input);
            var buffer = new byte[combined.Length];
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(passphrase)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                if (aes == null)
                {
                    throw new ArgumentException("Parameter must not be null.", nameof(aes));
                }

                aes.Key = aesKey;

                var iv = new byte[aes.IV.Length];
                var ciphertext = new byte[buffer.Length - iv.Length];

                Array.ConstrainedCopy(combined, 0, iv, 0, iv.Length);
                Array.ConstrainedCopy(combined, iv.Length, ciphertext, 0, ciphertext.Length);

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(ciphertext))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    return Encoding.UTF8.GetString(resultStream.ToArray());
                }
            }
        }
    }
}
