using Structr.IO.Internal;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.IO
{
    public static class FileHelper
    {
        /// <summary>
        /// Save byte array by absolue path.
        /// </summary>
        /// <param name="path">Absolute path.</param>
        /// <param name="bytes">Byte array to save.</param>
        /// <param name="createDirIfNotExists">Flag indicates to create destination directory if not exists.</param>
        /// <param name="overrideFileIfExists">Flag indicates to override destination file if exists.</param>
        /// <returns></returns>
        public static string SaveFile(string path,
            byte[] bytes,
            bool createDirIfNotExists = true,
            bool overrideFileIfExists = false)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            string filePath = path;

            filePath = RemoveInvalidFileNameChars(filePath);
            string dir = Path.GetDirectoryName(filePath);

            if (!System.IO.Directory.Exists(dir))
            {
                if (!createDirIfNotExists)
                    throw new InvalidOperationException($"Directory {dir} was not found");

                System.IO.Directory.CreateDirectory(dir);
            }

            if (File.Exists(filePath))
            {
                if (!overrideFileIfExists)
                    throw new InvalidOperationException($"File {filePath} already exists");

                filePath = GetFilePathWithUniqueFileName(filePath);
            }

            File.WriteAllBytes(filePath, bytes);

            return filePath;
        }

        /// <summary>
        /// Save byte array by absolue path.
        /// </summary>
        /// <param name="path">Absolute path.</param>
        /// <param name="bytes">Byte array to save.</param>
        /// <param name="createDirIfNotExists">Flag indicates to create destination directory if not exists.</param>
        /// <param name="overrideFileIfExists">Flag indicates to override destination file if exists.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Absolute file path.</returns>
        public static async Task<string> SaveFileAsync(string path,
            byte[] bytes,
            bool createDirIfNotExists = true,
            bool overrideFileIfExists = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            string filePath = path;

            filePath = RemoveInvalidFileNameChars(filePath);
            string dir = Path.GetDirectoryName(filePath);

            if (!System.IO.Directory.Exists(dir))
            {
                if (!createDirIfNotExists)
                    throw new InvalidOperationException($"Directory {dir} was not found");

                System.IO.Directory.CreateDirectory(dir);
            }

            if (File.Exists(filePath))
            {
                if (!overrideFileIfExists)
                    throw new InvalidOperationException($"File {filePath} already exists");

                filePath = GetFilePathWithUniqueFileName(filePath);
            }

            await AsyncFile.WriteAllBytesAsync(filePath, bytes, cancellationToken);

            return filePath;
        }

        /// <summary>
        /// Read file into byte array.
        /// </summary>
        /// <param name="path">Absolute file path.</param>
        /// <param name="throwIfNotExists">Flag indicates to throw exception if file not exists.</param>
        /// <returns></returns>
        public static byte[] ReadFile(string path, bool throwIfNotExists = true)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
            {
                if (throwIfNotExists)
                    throw new InvalidOperationException($"File {path} not found");
                else
                    return null;
            }

            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// Read file into byte array.
        /// </summary>
        /// <param name="path">Absolute file path.</param>
        /// <param name="throwIfNotExists">Flag indicates to throw exception if file not exists.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        public static async Task<byte[]> ReadFileAsync(string path,
            bool throwIfNotExists = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
            {
                if (throwIfNotExists)
                    throw new InvalidOperationException($"File {path} not found");
                else
                    return null;
            }

            return await AsyncFile.ReadAllBytesAsync(path, cancellationToken);
        }

        /// <summary>
        /// Read file from stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="initialLength">Length of returning byte array.</param>
        /// <returns></returns>
        public static byte[] ReadFile(Stream stream, long initialLength = 0)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

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
        /// Read file from stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="initialLength">Length of returning byte array.</param>
        /// <returns></returns>
        public static async Task<byte[]> ReadFileAsync(Stream stream,
            long initialLength = 0,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

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
        /// Delete file with checking  for exists.
        /// </summary>
        /// <param name="path">Absolute file path.</param>
        public static void DeleteFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            if (File.Exists(path))
                File.Delete(path);
        }

        /// <summary>
        /// Get absolute path with changing destination file name with unique postfix e.g. ("file_1.exe", "file_2.exe").
        /// </summary>
        /// <param name="path">Absolute file path.</param>
        /// <returns></returns>
        public static string GetFilePathWithUniqueFileName(string path)
        {
            string fileNameOnly = Path.GetFileNameWithoutExtension(path),
                fileExtension = Path.GetExtension(path),
                dir = Path.GetDirectoryName(path);

            if (File.Exists(path))
            {
                string tempFileName = "";
                string pathToCheck = path;
                int counter = 1;
                while (File.Exists(pathToCheck))
                {
                    tempFileName = $"{fileNameOnly}_{counter.ToString()}{fileExtension}";
                    pathToCheck = Path.Combine(dir, tempFileName);
                    counter++;
                }

                fileNameOnly = Path.GetFileNameWithoutExtension(tempFileName);
            }

            return Path.Combine(dir, fileNameOnly + fileExtension);
        }

        private static string RemoveInvalidFileNameChars(string path)
        {
            string fileNameOnly = Path.GetFileNameWithoutExtension(path),
                fileExtension = Path.GetExtension(path),
                dir = Path.GetDirectoryName(path);

            fileNameOnly = fileNameOnly.Trim(Path.GetInvalidFileNameChars());

            return Path.Combine(dir, fileNameOnly + fileExtension);
        }
    }
}
