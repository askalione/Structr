using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.IO.Internal
{
    internal static class AsyncFile
    {
        private static Encoding _UTF8NoBOM;
        internal const int DefaultBufferSize = 4096;

        private static Encoding UTF8NoBOM => _UTF8NoBOM ?? (_UTF8NoBOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));

        public static Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (path.Length == 0)
                throw new ArgumentException("Invalid file path.", nameof(path));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            return cancellationToken.IsCancellationRequested
                ? Task.FromCanceled(cancellationToken)
                : InternalWriteAllBytesAsync(path, bytes, cancellationToken);
        }

        private static async Task InternalWriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
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

        public static Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<byte[]>(cancellationToken);
            }

            var fs = new FileStream(path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 1, // bufferSize == 1 used to avoid unnecessary buffer in FileStream
                useAsync: true);

            bool returningInternalTask = false;
            try
            {
                long fileLength = fs.Length;
                if (fileLength > int.MaxValue)
                {
                    return Task.FromException<byte[]>(new IOException(
                        "The file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size."));
                }

                if (!(fileLength > 0))
                    throw new IOException("Invalid file length.");

                returningInternalTask = true;
                return InternalReadAllBytesAsync(fs, (int)fileLength, cancellationToken);
            }
            finally
            {
                if (!returningInternalTask)
                    fs.Dispose();
            }
        }

        private static async Task<byte[]> InternalReadAllBytesAsync(FileStream fs, int count, CancellationToken cancellationToken)
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

        public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken))
            => WriteAllLinesAsync(path, contents, UTF8NoBOM, cancellationToken);

        public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (contents == null)
                throw new ArgumentNullException(nameof(contents));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            if (path.Length == 0)
                throw new ArgumentException("Invalid file path.", nameof(path));

            return cancellationToken.IsCancellationRequested
                ? Task.FromCanceled(cancellationToken)
                : InternalWriteAllLinesAsync(AsyncStreamWriter(path, encoding, append: false), contents, cancellationToken);
        }

        private static async Task InternalWriteAllLinesAsync(TextWriter writer, IEnumerable<string> contents, CancellationToken cancellationToken)
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

        public static Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
            => ReadAllTextAsync(path, Encoding.UTF8, cancellationToken);

        public static Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            if (path.Length == 0)
                throw new ArgumentException("Invalid file path.", nameof(path));

            return cancellationToken.IsCancellationRequested
                ? Task.FromCanceled<string>(cancellationToken)
                : InternalReadAllTextAsync(path, encoding, cancellationToken);
        }

        private static async Task<string> InternalReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken)
        {
            using (var sr = AsyncStreamReader(path, encoding))
            {
                cancellationToken.ThrowIfCancellationRequested();
                StringBuilder sb = new StringBuilder();
                char[] buffer = new char[0x1000];
                int read;
                while ((read = await sr.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    sb.Append(buffer, 0, read);

                return sb.ToString();
            }
        }

        public static Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken))
            => WriteAllTextAsync(path, contents, UTF8NoBOM, cancellationToken);

        public static Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            if (path.Length == 0)
                throw new ArgumentException("Invalid file path.", nameof(path));

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

        private static async Task InternalWriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken)
        {

            using (var sw = AsyncStreamWriter(path, encoding, append: false))
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
            FileStream stream = new FileStream(
                path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, useAsync: true);

            return new StreamWriter(stream, encoding);
        }

        private static StreamReader AsyncStreamReader(string path, Encoding encoding)
        {
            FileStream stream = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, useAsync: true);

            return new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true);
        }
    }
}
