using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.IO.Internal
{
    /// <summary>
    /// Provides asynchronous methods for the read and write a single file.
    /// </summary>
    internal static class AsyncFile
    {
        private static Encoding _UTF8NoBOM;
        internal const int DefaultBufferSize = 4096;

        private static Encoding UTF8NoBOM => _UTF8NoBOM ?? (_UTF8NoBOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));

        /// <summary>
        /// Asynchronously creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The absolute file path to write to.</param>
        /// <param name="bytes">The bytes to write to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="bytes"/> is <see langword="null"/>.</exception>
        public static Task WriteAllBytesAsync(
            string path,
            byte[] bytes,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "Invalid file path.");
            }
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            Task result;
            if (cancellationToken.IsCancellationRequested)
            {
                result = Task.FromCanceled(cancellationToken);
            }
            else
            {
                result = InternalWriteAllBytesAsync(path, bytes, cancellationToken);
            }
            return result;
        }

        private static async Task InternalWriteAllBytesAsync(
            string path,
            byte[] bytes,
            CancellationToken cancellationToken)
        {
            using (FileStream fs = new FileStream(path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.Read,
                DefaultBufferSize,
                useAsync: true))
            {
                await fs.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
                await fs.FlushAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Asynchronously opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">The absolute file path to open for reading.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous read operation, which wraps the byte array containing the contents of the file.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="IOException">If file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size.</exception>
        public static Task<byte[]> ReadAllBytesAsync(
            string path,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "Invalid file path.");
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<byte[]>(cancellationToken);
            }

            // NOTE: bufferSize == 1 used to avoid unnecessary buffer in FileStream
            var fs = new FileStream(path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 1,
                useAsync: true);

            bool returningInternalTask = false;
            try
            {
                long fileLength = fs.Length;
                if (fileLength > int.MaxValue)
                {
                    var message = "The file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size.";
                    return Task.FromException<byte[]>(new IOException(message));
                }

                if (fileLength <= 0)
                {
                    throw new IOException("Invalid file length.");
                }

                returningInternalTask = true;
                Task<byte[]> result = InternalReadAllBytesAsync(fs, (int)fileLength, cancellationToken);
                return result;
            }
            finally
            {
                if (returningInternalTask == false)
                {
                    fs.Dispose();
                }
            }
        }

        private static async Task<byte[]> InternalReadAllBytesAsync(
            FileStream fs,
            int count,
            CancellationToken cancellationToken)
        {
            using (fs)
            {
                int index = 0;
                byte[] bytes = new byte[count];
                do
                {
                    int n = await fs.ReadAsync(bytes, index, count - index, cancellationToken);

                    if (n == 0)
                    {
                        throw new EndOfStreamException("Unable to read beyond the end of the stream.");
                    }

                    index += n;
                } while (index < count);

                return bytes;
            }
        }

        /// <inheritdoc cref="WriteAllLinesAsync(string, IEnumerable{string}, Encoding, CancellationToken)"/>
        public static Task WriteAllLinesAsync(
            string path,
            IEnumerable<string> contents,
            CancellationToken cancellationToken = default(CancellationToken))
            => WriteAllLinesAsync(path, contents, UTF8NoBOM, cancellationToken);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified lines to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The absolute file path to write to.</param>
        /// <param name="contents">The lines to write to the file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="contents"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="encoding"/> is <see langword="null"/>.</exception>
        public static Task WriteAllLinesAsync(
            string path,
            IEnumerable<string> contents,
            Encoding encoding,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "Invalid file path.");
            }
            if (contents == null)
            {
                throw new ArgumentNullException(nameof(contents));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            Task result;
            if (cancellationToken.IsCancellationRequested)
            {
                result = Task.FromCanceled(cancellationToken);
            }
            else
            {
                result = InternalWriteAllLinesAsync(AsyncStreamWriter(path, encoding, append: false), contents, cancellationToken);
            }
            return result;
        }

        private static async Task InternalWriteAllLinesAsync(
            TextWriter writer,
            IEnumerable<string> contents,
            CancellationToken cancellationToken)
        {
            using (writer)
            {
                foreach (string line in contents)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await writer.WriteLineAsync(line);
                }

                cancellationToken.ThrowIfCancellationRequested();
                await writer.FlushAsync();
            }
        }

        /// <inheritdoc cref="ReadAllTextAsync(string, Encoding, CancellationToken)"/>
        public static Task<string> ReadAllTextAsync(
            string path,
            CancellationToken cancellationToken = default(CancellationToken))
            => ReadAllTextAsync(path, Encoding.UTF8, cancellationToken);

        /// <summary>
        /// Asynchronously opens a text file, reads all the text in the file, and then closes the file.
        /// </summary>
        /// <param name="path">The absolute file path to open for reading.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous read operation, which wraps the string containing all text in the file.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="encoding"/> is <see langword="null"/>.</exception>
        public static Task<string> ReadAllTextAsync(
            string path,
            Encoding encoding,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "Invalid file path.");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            Task<string> result;
            if (cancellationToken.IsCancellationRequested)
            {
                result = Task.FromCanceled<string>(cancellationToken);
            }
            else
            {
                result = InternalReadAllTextAsync(path, encoding, cancellationToken);
            }
            return result;
        }

        private static async Task<string> InternalReadAllTextAsync(
            string path,
            Encoding encoding,
            CancellationToken cancellationToken)
        {
            using (StreamReader sr = AsyncStreamReader(path, encoding))
            {
                cancellationToken.ThrowIfCancellationRequested();
                StringBuilder sb = new StringBuilder();
                char[] buffer = new char[0x1000];
                int read;
                while ((read = await sr.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    sb.Append(buffer, 0, read);
                }

                var result = sb.ToString();
                return result;
            }
        }

        /// <inheritdoc cref="WriteAllTextAsync(string, string, Encoding, CancellationToken)"/>
        public static Task WriteAllTextAsync(
            string path,
            string contents,
            CancellationToken cancellationToken = default(CancellationToken))
            => WriteAllTextAsync(path, contents, UTF8NoBOM, cancellationToken);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The absolute file path to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        /// <param name="encoding">The encoding to apply to the string.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="encoding"/> is <see langword="null"/>.</exception>
        public static Task WriteAllTextAsync(
            string path,
            string contents,
            Encoding encoding,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "Invalid file path.");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            if (string.IsNullOrEmpty(contents))
            {
                new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read).Dispose();
                return Task.CompletedTask;
            }

            return InternalWriteAllTextAsync(path, contents, encoding, cancellationToken);
        }

        private static async Task InternalWriteAllTextAsync(
            string path,
            string contents,
            Encoding encoding,
            CancellationToken cancellationToken)
        {
            using (StreamWriter sw = AsyncStreamWriter(path, encoding, append: false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                char[] buffer = new char[0x1000];
                int count = contents.Length;
                int index = 0;
                while (index < count)
                {
                    int batchSize = Math.Min(DefaultBufferSize, count - index);
                    contents.CopyTo(index, buffer, 0, batchSize);
                    await sw.WriteAsync(buffer, 0, batchSize);
                    index += batchSize;
                }
                cancellationToken.ThrowIfCancellationRequested();
                await sw.FlushAsync();
            }
        }

        private static StreamWriter AsyncStreamWriter(string path, Encoding encoding, bool append)
        {
            var fileMode = append ? FileMode.Append : FileMode.Create;
            FileStream stream = new FileStream(
                path, fileMode, FileAccess.Write, FileShare.Read, DefaultBufferSize, useAsync: true);

            var result = new StreamWriter(stream, encoding);
            return result;
        }

        private static StreamReader AsyncStreamReader(string path, Encoding encoding)
        {
            FileStream stream = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, useAsync: true);

            var result = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true);
            return result;
        }
    }
}
