using Structr.IO.Internal;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.IO
{
    /// <summary>
    /// Provides static synchronous and asynchronous methods for the save, read and delete a single file.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Synchronously saves a byte array to a file by an absolute path.
        /// </summary>
        /// <returns>The absolute path to the saved file.</returns>
        /// <inheritdoc cref="SaveFileAsync(string, byte[], bool, bool, CancellationToken)"/>
        public static string SaveFile(string path,
            byte[] bytes,
            bool createDirIfNotExists = true,
            bool overrideFileIfExists = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            string filePath = RemoveInvalidFileNameChars(path);

            string dir = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dir) == false)
            {
                if (createDirIfNotExists == false)
                {
                    throw new InvalidOperationException($"Directory {dir} was not found");
                }
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(filePath))
            {
                if (overrideFileIfExists == false)
                {
                    throw new InvalidOperationException($"File {filePath} already exists");
                }
                filePath = GetFilePathWithUniqueFileName(filePath);
            }

            File.WriteAllBytes(filePath, bytes);
            return filePath;
        }

        /// <summary>
        /// Asynchronously saves a byte array to a file by an absolute path.
        /// </summary>
        /// <param name="path">The absolute file path to save to.</param>
        /// <param name="bytes">The bytes to save to the file.</param>
        /// <param name="createDirIfNotExists">The flag indicates to create destination directory if not exists.</param>
        /// <param name="overrideFileIfExists">The flag indicates to override destination file if exists.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous save operation, which wraps the absolute path to the saved file</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="bytes"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">If directory <paramref name="dir"/> was not found and <paramref name="createDirIfNotExists"/> is <see langword="false"/>.</exception>
        /// <exception cref="InvalidOperationException">If file <paramref name="filePath"/> already exists and <paramref name="overrideFileIfExists"/> is <see langword="false"/>.</exception>
        public static async Task<string> SaveFileAsync(string path,
            byte[] bytes,
            bool createDirIfNotExists = true,
            bool overrideFileIfExists = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            string filePath = RemoveInvalidFileNameChars(path);

            string dir = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dir) == false)
            {
                if (createDirIfNotExists == false)
                {
                    throw new InvalidOperationException($"Directory {dir} was not found");
                }
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(filePath))
            {
                if (overrideFileIfExists == false)
                {
                    throw new InvalidOperationException($"File {filePath} already exists");
                }
                filePath = GetFilePathWithUniqueFileName(filePath);
            }

            await AsyncFile.WriteAllBytesAsync(filePath, bytes, cancellationToken);
            return filePath;
        }

        /// <summary>
        /// Synchronously reads a file from an absolute path to a byte array.
        /// </summary>
        /// <returns>The byte array containing the contents of the file.</returns>
        /// <inheritdoc cref="ReadFileAsync(string, bool, CancellationToken)"/>
        public static byte[] ReadFile(string path, bool throwIfNotExists = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (File.Exists(path) == false)
            {
                if (throwIfNotExists)
                {
                    throw new InvalidOperationException($"File {path} not found");
                }
                else
                {
                    return null;
                }
            }

            var result = File.ReadAllBytes(path);
            return result;
        }

        /// <summary>
        /// Asynchronously reads a file from an absolute path to a byte array.
        /// </summary>
        /// <param name="path">The absolute file path to read to.</param>
        /// <param name="throwIfNotExists">The flag indicates to throw exception if file not exists.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous read operation, which wraps the byte array containing the contents of the file.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="InvalidOperationException">If file <paramref name="filePath"/> not exists and <paramref name="throwIfNotExists"/> is <see langword="true"/>.</exception>
        public static async Task<byte[]> ReadFileAsync(string path,
            bool throwIfNotExists = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (File.Exists(path) == false)
            {
                if (throwIfNotExists)
                {
                    throw new InvalidOperationException($"File {path} not found");
                }
                else
                {
                    return null;
                }
            }

            var result = await AsyncFile.ReadAllBytesAsync(path, cancellationToken);
            return result;
        }

        /// <summary>
        /// Synchronously reads a file from a stream to a byte array.
        /// </summary>
        /// <returns>The byte array from the <paramref name="stream"/>.</returns>
        /// <inheritdoc cref="ReadFileAsync(Stream, long, CancellationToken)"/>
        public static byte[] ReadFile(Stream stream, long initialLength = 0)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            // reset pointer just in case
            stream.Seek(0, SeekOrigin.Begin);

            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        /// <summary>
        /// Asynchronously reads a file from a stream to a byte array.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/>.</param>
        /// <param name="initialLength">Length of returning byte array.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous read operation, which wraps the byte array from the <paramref name="stream"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is <see langword="null"/>.</exception>
        public static async Task<byte[]> ReadFileAsync(Stream stream,
            long initialLength = 0,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            // reset pointer just in case
            stream.Seek(0, SeekOrigin.Begin);

            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = await stream.ReadAsync(buffer, read, buffer.Length - read, cancellationToken)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        /// <summary>
        /// Deletes a file if it exists.
        /// </summary>
        /// <param name="path">The absolute file path to delete to.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public static void DeleteFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Returns an absolute path with changing destination file name with unique postfix e.g. ("file_1.exe", "file_2.exe").
        /// </summary>
        /// <param name="path">The absolute file path.</param>
        public static string GetFilePathWithUniqueFileName(string path)
        {
            var fileNameOnly = Path.GetFileNameWithoutExtension(path);
            var fileExtension = Path.GetExtension(path);
            var dir = Path.GetDirectoryName(path);

            if (File.Exists(path))
            {
                var tempFileName = "";
                var pathToCheck = path;
                int counter = 1;
                while (File.Exists(pathToCheck))
                {
                    tempFileName = $"{fileNameOnly}_{counter}{fileExtension}";
                    pathToCheck = Path.Combine(dir, tempFileName);
                    counter++;
                }

                fileNameOnly = Path.GetFileNameWithoutExtension(tempFileName);
            }

            var result = Path.Combine(dir, fileNameOnly + fileExtension);
            return result;
        }

        private static string RemoveInvalidFileNameChars(string path)
        {
            var fileNameOnly = Path.GetFileNameWithoutExtension(path);
            var fileExtension = Path.GetExtension(path);
            var dir = Path.GetDirectoryName(path);

            fileNameOnly = fileNameOnly.Trim(Path.GetInvalidFileNameChars());

            var result = Path.Combine(dir, fileNameOnly + fileExtension);
            return result;
        }
    }
}
